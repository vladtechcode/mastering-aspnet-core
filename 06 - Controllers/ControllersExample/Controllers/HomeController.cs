using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers;
/*
 * The [Controller] attribute is a marker attribute in ASP.NET Core MVC that
 * explicitly indicates a class should be treated as a controller by the framework.
 */
[Controller]
public class HomeController : Controller
{
    // GET
    [Route("/")]
    [Route("/index")]
    public IActionResult Index()
    {
        /*
         * ContentResult is an action result type in ASP.NET Core MVC that returns raw
         * content (text, HTML, JSON, XML, etc.) directly to the HTTP response.
         *
         * Key Properties
         *
         * Content (string): The actual content to return in the response body
         * ContentType (string): The MIME type of the content (e.g., "text/plain", "text/html", "application/json")
         * StatusCode (int?): Optional HTTP status code (defaults to 200 OK if not specified)
         *
         * Common Use Cases
         * 1. Plain Text``` csharp
         * return new ContentResult()
         * {
         * Content = "Hello World",
         * ContentType = "text/plain"
         * };
         * ```
         * 2. HTML Content``` csharp
         * return new ContentResult()
         * {
         * Content = "<h1>Welcome</h1><p>This is HTML content</p>",
         * ContentType = "text/html"
         * };
         * ```
         * 3. JSON (manual)``` csharp
         * return new ContentResult()
         * {
         * Content = "{\"name\":\"John\",\"age\":30}",
         * ContentType = "application/json"
         * };
         * ``` 
         */
        return new ContentResult()
        {
            Content = "This is an basic example of a controller from index action method",
            ContentType = "text/plain",
            StatusCode = 200,
        };
    }
    
    //Get
    [Route("/about")]
    public IActionResult About()
    {
        return Content("This is an basic example of a controller from about action method", "text/plain");
    }
    
    //GET
    [Route("/contact-us")]
    public IActionResult Contact()
    {
        return Content("This is an basic example of a controller from contact action method", "text/plain");
    }
}