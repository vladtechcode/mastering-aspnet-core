`UseWhen` is a middleware branching method in ASP.NET Core that allows you to conditionally execute a separate middleware pipeline based on the result of a predicate function, while still rejoining the main pipeline afterward.

## How it Works

`UseWhen` creates a **branch** in the middleware pipeline that:
1. Evaluates a condition on each request
2. If the condition is true, executes an alternate middleware pipeline
3. **Rejoins the main pipeline** after the branch completes (this is the key difference from `MapWhen`)

## Syntax

```csharp
app.UseWhen(
    context => /* condition */,
    appBuilder => 
    {
        // Configure middleware for this branch
    }
);
```

## Example

```csharp
app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api"),
    appBuilder => 
    {
        appBuilder.UseMiddleware<ApiLoggingMiddleware>();
        appBuilder.UseMiddleware<ApiAuthenticationMiddleware>();
    }
);

// This middleware runs for ALL requests, including those that went through the branch
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
```

## Key Characteristics

- **Non-terminal**: The pipeline continues after the branch
- **Conditional execution**: Only runs the branch if the predicate is true
- **Rejoins main pipeline**: Unlike `MapWhen`, requests return to the main pipeline

## Common Use Cases

- Adding logging for specific paths
- Applying authentication only to certain routes
- Adding custom headers based on request properties
- Performance monitoring for specific endpoints

## UseWhen vs MapWhen

- **UseWhen**: Returns to main pipeline (non-terminal)
- **MapWhen**: Creates a separate pipeline that doesn't rejoin (terminal)