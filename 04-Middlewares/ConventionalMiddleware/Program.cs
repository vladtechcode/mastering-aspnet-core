using ConventionalMiddleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Use the custom conventional middleware
app.UseMyConventionalMiddleware();

app.Run();
