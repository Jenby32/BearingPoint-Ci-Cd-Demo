var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

app.MapGet("/api/info", () => Results.Ok(new
{
    message = "Hello BearingPoint CI/CD!",
    environment = builder.Environment.EnvironmentName,
    timestamp = DateTime.Now
}));


app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.Now
}));

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
