namespace ConventionalMiddleware.Middlewares
{
    /** ## Conventional Middleware
     * This is the traditional way of creating middleware in ASP.NET Core. 
     * A conventional middleware class typically:
     * 
     * 1.  Has a constructor that takes a `RequestDelegate` parameter, 
     *     which represents the next middleware in the pipeline.
     * 2.  Contains an `Invoke` or `InvokeAsync` method that takes an 
     *     `HttpContext` parameter and returns a `Task`. This method is 
     *     called to process HTTP requests.
     * 
     * Conventional middleware is usually registered as a singleton in 
     * the application's request pipeline using extension methods on 
     * `IApplicationBuilder`.
     */
    public class MyConventionalMiddleware
    {
        // Field to hold the next middleware in the pipeline
        private readonly RequestDelegate _next;

        // Constructor that takes the next middleware as a parameter
        public MyConventionalMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context) 
        {
            await context.Response.WriteAsync("Hello from ConventionalMiddleware");
            // Example of checking for a specific query parameter
            if (context.Request.Query.ContainsKey("lastname"))
            {
                // Example of reading query parameters
                await context.Response.WriteAsync($"\nHello {context.Request.Query["lastname"]}");
            }
            await _next(context);
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
    public static class MyConventionalMiddlewareExtensions 
    {
        // Extension method to register the middleware in the pipeline
        public static IApplicationBuilder UseMyConventionalMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyConventionalMiddleware>();
        }
    }




}
