**Factory-based middleware** (also called **IMiddleware**) is an alternative approach to creating middleware in ASP.NET Core where middleware instances are created per request using a factory, rather than being singleton instances like conventional middleware.

## The IMiddleware Interface

Factory-based middleware implements the `IMiddleware` interface:

```csharp
public interface IMiddleware
{
    Task InvokeAsync(HttpContext context, RequestDelegate next);
}
```

## Basic Implementation

```csharp
public class MyFactoryMiddleware : IMiddleware
{
    private readonly ILogger<MyFactoryMiddleware> _logger;

    // Dependencies injected via constructor
    public MyFactoryMiddleware(ILogger<MyFactoryMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Before next middleware");
        
        await next(context);
        
        _logger.LogInformation("After next middleware");
    }
}
```

## Registration

Factory-based middleware requires **two steps**:

```csharp
// 1. Register the middleware with DI container
builder.Services.AddScoped<MyFactoryMiddleware>(); // or Transient/Singleton

// 2. Add to pipeline
app.UseMiddleware<MyFactoryMiddleware>();
```

## Key Differences from Conventional Middleware

| Feature | Conventional | Factory-Based (IMiddleware) |
|---------|-------------|----------------------------|
| **Lifetime** | Singleton (one instance) | Per DI registration (typically Scoped) |
| **DI Services** | Constructor: Singleton only<br>InvokeAsync: Any lifetime | Constructor: Any lifetime |
| **Interface** | None (convention-based) | IMiddleware interface |
| **Registration** | Just pipeline | DI container + pipeline |
| **Creation** | Once at startup | Per request (if scoped) |
| **Performance** | Faster (no activation cost) | Slightly slower (activation per request) |

## Why Use Factory-Based Middleware?

### 1. **Scoped Services in Constructor**

```csharp
public class DatabaseLoggingMiddleware : IMiddleware
{
    private readonly ApplicationDbContext _dbContext; // Scoped service!

    // Can inject scoped services directly
    public DatabaseLoggingMiddleware(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await _dbContext.Logs.AddAsync(new Log 
        { 
            Path = context.Request.Path,
            Timestamp = DateTime.UtcNow 
        });
        
        await next(context);
        
        await _dbContext.SaveChangesAsync();
    }
}

// Register as scoped
builder.Services.AddScoped<DatabaseLoggingMiddleware>();
app.UseMiddleware<DatabaseLoggingMiddleware>();
```

### 2. **Stateful Middleware**

Since instances are created per request, you can safely store request-specific state:

```csharp
public class RequestContextMiddleware : IMiddleware
{
    private Guid _requestId; // Safe to use instance fields!

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _requestId = Guid.NewGuid();
        context.Items["RequestId"] = _requestId;
        
        await next(context);
    }
}
```

### 3. **Easier Testing**

```csharp
public class MyMiddlewareTests
{
    [Fact]
    public async Task TestMiddleware()
    {
        // Easy to instantiate and test
        var logger = Mock.Of<ILogger<MyFactoryMiddleware>>();
        var middleware = new MyFactoryMiddleware(logger);
        
        var context = new DefaultHttpContext();
        var nextCalled = false;
        RequestDelegate next = (ctx) => 
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        
        await middleware.InvokeAsync(context, next);
        
        Assert.True(nextCalled);
    }
}
```

## Complete Real-World Example

```csharp
// Middleware that tracks request metrics using EF Core
public class RequestMetricsMiddleware : IMiddleware
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RequestMetricsMiddleware> _logger;

    public RequestMetricsMiddleware(
        ApplicationDbContext dbContext, // Scoped service
        ILogger<RequestMetricsMiddleware> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            
            var metric = new RequestMetric
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                Duration = stopwatch.ElapsedMilliseconds,
                Timestamp = DateTime.UtcNow
            };
            
            _dbContext.RequestMetrics.Add(metric);
            
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save request metrics");
            }
        }
    }
}

// Registration
builder.Services.AddScoped<RequestMetricsMiddleware>();
app.UseMiddleware<RequestMetricsMiddleware>();
```

## Custom Factory

You can also implement `IMiddlewareFactory` for complete control:

```csharp
public class CustomMiddlewareFactory : IMiddlewareFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CustomMiddlewareFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IMiddleware Create(Type middlewareType)
    {
        // Custom logic to create middleware
        return _serviceProvider.GetRequiredService(middlewareType) as IMiddleware;
    }

    public void Release(IMiddleware middleware)
    {
        // Cleanup logic
        if (middleware is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}

// Register custom factory
builder.Services.AddSingleton<IMiddlewareFactory, CustomMiddlewareFactory>();
```

## When to Use Factory-Based Middleware

**Use Factory-Based (IMiddleware) when:**
- Need to inject scoped services (like DbContext, EF Core)
- Need request-specific state in fields
- Want easier unit testing
- Prefer explicit interface implementation

**Use Conventional when:**
- Performance is critical (avoid activation overhead)
- Only need singleton dependencies
- Following ASP.NET Core conventions
- Building framework-level middleware

## Performance Consideration

Factory-based middleware has a slight performance cost due to:
- Activation per request
- Service resolution overhead
- Potential disposal

For most applications, this overhead is negligible, but for high-throughput scenarios, conventional middleware may be preferred.