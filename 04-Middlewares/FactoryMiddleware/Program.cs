using FactoryMiddleware.Middlewares;

var builder = WebApplication.CreateBuilder(args);

/*
 * Register the middleware as a transient service and allows the 
 * middleware to be created each time it's requested.In contrast to
 * conventional middleware, which is typically a singleton, 
 * factory-based middleware can have dependencies with different
 * lifetimes and needs to be registered in the DI container.
 */
builder.Services.AddTransient<MyFactoryMiddleware>();

var app = builder.Build();

app.UseMyFactoryMiddleware();

app.Run();
