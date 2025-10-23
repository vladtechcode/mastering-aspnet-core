
namespace FactoryMiddleware.Middlewares
{
    /*
     * ## Factory-Based Middleware
     * This approach uses an interface, `IMiddleware`, and is activated 
     * by the application's dependency injection (DI) container. This 
     * means the middleware itself is registered as a service, allowing 
     * it to have its own dependencies with different lifetimes 
     * (like **scoped** or **transient**). This is its main advantage.
     * 
     * A factory-based middleware class must:
     * 
     * 1.  Implement the `IMiddleware` interface.
     * 2.  Provide an implementation for the `InvokeAsync` method,
     *     which takes `HttpContext` and `RequestDelegate` as parameters.
     */
    public class MyFactoryMiddleware : IMiddleware
    {
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("\nHello from MyFactoryMiddleware");
            await next(context);
        }

    }

    /* 
     * 
     * It creates a simple helper method that makes adding your middleware
     * to the Program.cs file cleaner, more readable, and more expressive.
     * It's all about improving the developer experience.
     * 
     * The goal is to provide syntactic sugar. Instead of forcing
     * developers to use the generic UseMiddleware method and remember
     * the exact class name, you provide a clean, descriptive method.
     * 
     */
    public static class MyFactoryMiddlewareExtensions
    {
        // Extension method to register the middleware in the pipeline
        public static IApplicationBuilder UseMyFactoryMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyFactoryMiddleware>();
        }
    }
}
