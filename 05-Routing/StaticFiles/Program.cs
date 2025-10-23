var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Add static files middleware
app.UseStaticFiles();
app.MapGet("/", () => "Hello World!");

app.Run();