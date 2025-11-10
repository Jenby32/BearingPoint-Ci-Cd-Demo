var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
