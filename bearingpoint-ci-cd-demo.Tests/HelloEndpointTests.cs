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
    private const int Port = 5005;
    private const string BaseUrl = "http://localhost:5014";

    public async Task InitializeAsync()
    {
        // Pfad zu deinem API-Projekt
        var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, ".." ,"..", "..", "..", "bearingpoint-ci-cd-demo.Api", "bearingpoint-ci-cd-demo.csproj"));

        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                // WICHTIG: --no-build, damit er NICHT nochmal kompiliert → kein Lock
                Arguments = $"run --no-build --urls {BaseUrl} --project \"{projectPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        _process.Start();

        // Optional: Output einsammeln, falls was schiefgeht
        var stdoutTask = _process.StandardOutput.ReadToEndAsync();
        var stderrTask = _process.StandardError.ReadToEndAsync();

        // Warten, bis /health erreichbar ist (max. 15s)
        for (var i = 0; i < 15; i++)
        {
            try
            {
                var resp = await _client.GetAsync($"{BaseUrl}/health");
                if (resp.IsSuccessStatusCode)
                    return; // API läuft
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
