using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Xunit;

namespace bearingpoint_ci_cd_demo.Tests;

public class ApiEndToEndTests : IAsyncLifetime
{
    private Process? _process;
    private readonly HttpClient _client = new();
    private const string BaseUrl = "http://localhost:5014";

    public async Task InitializeAsync()
    {
        // Pfad zum API-Projektordner
        var projectDir = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..",
                "bearingpoint-ci-cd-demo.Api"));

        if (!Directory.Exists(projectDir))
        {
            throw new DirectoryNotFoundException(
                $"API-Projektordner nicht gefunden: {projectDir}");
        }

        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                // Entweder explizit URL setzen:
                Arguments = $"run --urls {BaseUrl}",
                // oder Launch-Profile nutzen:
                // Arguments = "run --no-build --launch-profile bearingpoint_ci_cd_demo",
                WorkingDirectory = projectDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        _process.Start();

        var stdoutTask = _process.StandardOutput.ReadToEndAsync();
        var stderrTask = _process.StandardError.ReadToEndAsync();

        // Bis zu 15 Sekunden warten, bis /health erreichbar ist
        for (var i = 0; i < 15; i++)
        {
            if (_process.HasExited)
            {
                var stdoutEarly = await stdoutTask;
                var stderrEarly = await stderrTask;

                throw new Exception(
                    $"API-Prozess ist vorzeitig beendet (ExitCode: {_process.ExitCode}).\n\n" +
                    $"STDOUT:\n{stdoutEarly}\n\nSTDERR:\n{stderrEarly}");
            }

            try
            {
                var resp = await _client.GetAsync($"{BaseUrl}/health");
                if (resp.IsSuccessStatusCode)
                    return;
            }
            catch
            {
                // Noch nicht bereit
            }

            await Task.Delay(1000);
        }

        var stdout = await stdoutTask;
        var stderr = await stderrTask;
        throw new Exception(
            "API ist nicht gestartet worden.\n\nSTDOUT:\n" + stdout + "\n\nSTDERR:\n" + stderr);
    }

    public Task DisposeAsync()
    {
        if (_process is { HasExited: false })
        {
            _process.Kill(true);
        }
        _client.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Root_Returns_BearingPoint_Message()
    {
        var response = await _client.GetAsync($"{BaseUrl}/");
        var body = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("BearingPoint", body);
    }
}
