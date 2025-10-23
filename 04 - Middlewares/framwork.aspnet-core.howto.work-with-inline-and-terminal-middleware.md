---
id: 2mwdhg5i08w07hts2heug73
title: Work with Inline and Terminal Middleware
desc: ''
updated: 1759772705019
created: 1759772681497
---
```c#
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//middleware 1
app.Use(async (httpContext,next) =>
{
     await httpContext.Response.WriteAsync("Hello from Middleware 1\n");
     await next(httpContext);
});

//middleware 2
app.Use(async (httpContext,next) =>
{
    await httpContext.Response.WriteAsync("Hello again from Middleware 2\n");
    await next(httpContext);
});

//terminal middleware
app.Run(async (httpContext) =>
{
    await httpContext.Response.WriteAsync("Hello from terminal middleware\n");
});

app.Run();```
