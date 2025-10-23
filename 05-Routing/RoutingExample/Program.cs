var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/*
 * Routing is automatically enabled in ASP.NET Core 6+
 * No need for app.UseRouting() and app.UseEndpoints() anymore
 * Endpoints are defined directly on the app object
 */


app.MapGet("/map1", async (context) => {
    await context.Response.WriteAsync("Mapped to /map1");
});

app.MapPost("map2", async (context) => {
    await context.Response.WriteAsync("Mapped to /map2");
});

app.Map("map3", async (context) => {
    await context.Response.WriteAsync("Mapped to /map3");
});


//Fallback for any other requests
app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("Fallback endpoint");
});

app.Run();
