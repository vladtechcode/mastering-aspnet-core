# The app.Use() Method in ASP.NET Core

The `app.Use()` method is a fundamental part of ASP.NET Core's middleware pipeline. It allows you to add custom middleware components that can process HTTP requests and responses.

## What is app.Use()?

`app.Use()` registers middleware in the application's request processing pipeline. Middleware components are executed in the order they're added, forming a chain where each component can:

1. Process the incoming HTTP request
2. Decide whether to pass the request to the next middleware
3. Process the outgoing HTTP response

## Basic Syntax

```csharp
app.Use(async (context, next) =>
{
    // Code before calling next() executes on the request path
    
    await next(); // Calls the next middleware in the pipeline
    
    // Code after calling next() executes on the response path
});
```

## Key Parameters

- **context**: An `HttpContext` object containing information about the current HTTP request and response
- **next**: A delegate representing the next middleware in the pipeline

## Common Use Cases

**Logging requests:**
```csharp
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});
```

**Adding custom headers:**
```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Custom-Header", "MyValue");
    await next();
});
```

**Short-circuiting the pipeline:**
```csharp
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/blocked")
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Access Denied");
        return; // Don't call next()
    }
    await next();
});
```

## Important Notes

- Always call `await next()` unless you intentionally want to short-circuit the pipeline
- Middleware order matters - components execute in the order they're registered
- `app.Use()` is different from `app.Run()`, which is terminal middleware that doesn't call next

The middleware pipeline is the backbone of request processing in ASP.NET Core, and `app.Use()` gives you fine-grained control over how requests flow through your application.

 