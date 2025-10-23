/*
 * Creates a WebApplicationBuilder instance
 * Sets up default configuration (appsettings.json, environment variables,
 * command line args)
 * Configures logging
 * Sets up the dependency injection container
 * Configures the web host (Kestrel server)
 * 
 * The args parameter:
 * Command-line arguments passed to your application
 * Example: dotnet run --urls "http://localhost:5000"
 * 
 */
var builder = WebApplication.CreateBuilder(args);

/*
 * Finalizes all configuration
 * Creates the service provider (locks the DI container)
 * Creates the WebApplication instance
 * After this, you cannot add more services to DI
 * Now you can configure the middleware pipeline
 */
var app = builder.Build();

/* 
 * https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-9.0
 * The extension method called Use is used to execute a 
 * non-terminating / short-circuiting  middleware that may / may not
 * forward the request to next middleware in the pipeline.
 * 
 */
// middleware 1
app.Use(async (context, next) =>
{
    // Set response content type and status code
    context.Response.ContentType = "text/plain";
    context.Response.StatusCode = StatusCodes.Status200OK;

    // Pre-processing logic before passing to the next middleware
    Console.WriteLine("Start - Middleware 1");
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await context.Response.WriteAsync("Start - Middleware 1");
    await next(context);
    // Post-processing logic after the next middleware has executed
    Console.WriteLine("End - Middleware 1");
    Console.WriteLine($"Response: {context.Response.StatusCode}");
    await context.Response.WriteAsync("\nEnd - Middleware 1");
});

// middleware 2
app.Use(async (context, next) =>
{
    // Pre-processing logic before passing to the next middleware
    Console.WriteLine("Start - Middleware 2");
    await context.Response.WriteAsync("\nStart - Middleware 2");
    await next(context);
    // Post-processing logic after the next middleware has executed
    Console.WriteLine("End - Middleware 2");
    Console.WriteLine($"Response: {context.Response.StatusCode}");
    await context.Response.WriteAsync("\nEnd - Middleware 2");
});

/*
 * `app.UseWhen()` is a powerful middleware that allows you to
 * **conditionally branch the middleware pipeline**. It creates a
 * temporary "detour" for a request, applying a specific set of
 * middleware only when a condition is met, before rejoining the
 * main pipeline.
 * 
 * How It Works
 * 
 * The logic is straightforward:
 * 
 * 1.  A **condition** (predicate) is checked against the incoming request (`HttpContext`).
 * 2.  If the condition is **true**, the request is passed through
 * a separate, smaller pipeline of middleware that you define.
 * 3.  After that branch is finished, the request **continues down 
 * the main pipeline** right where it left off.
 * 4.  If the condition is **false**, the branch is skipped entirely.
 * 
 */
// middleware 3
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/special"),
    appBuilder =>
    {
        appBuilder.Use(async (context, next) =>
        {
            // Special handling for paths starting with /special
            Console.WriteLine("Special Middleware for /special path");
            await context.Response.WriteAsync("\nSpecial Middleware for /special path");
            await next(context);
        });
    }
);

/*
 * No subsequent middleware will be executed, making it suitable for 
 * handling requests that don't need further processing.Request Handling:
 * It takes a RequestDelegate as a parameter, which is typically expressed
    as a lambda function that receives an HttpContext object.

    Common Use Cases: 
    
    Simple responses: When you want to return a basic response without
    routing or additional processing.

    Fallback handling: Placed at the end of the middleware pipeline
    to catch any requests that weren't handled by previous middleware.

    Testing/debugging: Quick way to verify the pipeline is working 
    before adding more complex middleware.

 */
app.Run(async (HttpContext context) =>
{

    // Access request information like path and method
    string path = context.Request.Path;
    string method = context.Request.Method;

    // Write response asynchronously
    await context.Response.WriteAsync($"\nHello World!\nPath: {path}, Method: {method}");
});

// Start the application and listen for incoming HTTP requests
app.Run();
