**Conventional middleware** (also called convention-based middleware) is the standard way of creating custom middleware in ASP.NET Core using a class that follows a specific naming convention rather than implementing an interface.

## Structure

A conventional middleware class must have:

1. **A constructor** that accepts `RequestDelegate` as a parameter
2. **An `InvokeAsync` or `Invoke` method** that:
    - Takes `HttpContext` as the first parameter
    - Can optionally have additional parameters (injected from DI)
    - Returns a `Task`

## Basic Example

```csharp
public class MyCustomMiddleware
{
    private readonly RequestDelegate _next;

    // Constructor receives the next middleware in the pipeline
    public MyCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // This method is called for each request
    public async Task InvokeAsync(HttpContext context)
    {
        // Code before calling next - executes on the way IN
        Console.WriteLine("Before next middleware");

        // Call the next middleware in the pipeline
        await _next(context);

        // Code after calling next - executes on the way OUT
        Console.WriteLine("After next middleware");
    }
}
```

## Registration

```csharp
// In Program.cs
app.UseMiddleware<MyCustomMiddleware>();

// Or create an extension method for cleaner syntax
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyCustomMiddleware>();
    }
}

// Then use:
app.UseMyCustomMiddleware();
```

## Dependency Injection

You can inject services into the `InvokeAsync` method:

```csharp
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger; // Constructor injection

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IMyService myService) // Method injection
    {
        _logger.LogInformation($"Request: {context.Request.Path}");
        
        // Use myService
        await myService.DoSomethingAsync();
        
        await _next(context);
    }
}
```

## Complete Real-World Example

```csharp
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Continue to next middleware
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            _logger.LogInformation(
                "Request {Method} {Path} completed in {ElapsedMs}ms with status {StatusCode}",
                context.Request.Method,
                context.Request.Path,
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode
            );
        }
    }
}
```

## Short-Circuit Example

You can stop the pipeline by NOT calling `_next`:

```csharp
public class MaintenanceModeMiddleware
{
    private readonly RequestDelegate _next;

    public MaintenanceModeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        bool isMaintenanceMode = true; // Check from config

        if (isMaintenanceMode)
        {
            context.Response.StatusCode = 503;
            await context.Response.WriteAsync("Site is under maintenance");
            // NOT calling _next - pipeline stops here
            return;
        }

        await _next(context);
    }
}
```

## Key Points

- **Convention-based**: No interface required, just follow the pattern
- **Singleton lifetime**: Middleware is instantiated once, so don't store request-specific data in fields
- **Constructor injection**: For singleton services only
- **Method injection**: For scoped/transient services in `InvokeAsync`
- **Order matters**: Middleware executes in the order registered

## Conventional vs IMiddleware

**Conventional** (most common):
- Per-application lifetime (singleton)
- Better performance (no activation cost per request)
- More flexible with DI

**IMiddleware** (interface-based):
- Per-request lifetime
- Explicit interface implementation
- Easier to test

Most ASP.NET Core middleware uses the conventional approach due to its performance benefits.