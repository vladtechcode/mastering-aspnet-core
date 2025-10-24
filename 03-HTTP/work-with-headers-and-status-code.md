```c#
WebApplicationBuilder builder = Webapplication.Builder(args);
WebApplication app = builder.Build()

app.Run(async (HttpContext context) => {

    context.Response.StatusCode = 200;
    context.Response.StatusCode = StatusCode.Status200Ok;
    context.Response.Headers["Status-Message"] = "Ok"

    context.Response.ContentType = "text/plain";
    context.Response.Headers.ContentType = "text/plain";
    content.Response.Headers["content-type"] = "text/plain";


    string path = context.Request.Path;
    string method = context.Request.Method; 

    await context.Response.WriteAsync("Hello, World");
    await content.Response.WriteAsync($"\nPath: {path}, Method: {method}");
});

app.Run();
```