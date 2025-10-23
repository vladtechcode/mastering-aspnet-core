using Microsoft.AspNetCore.Mvc;

namespace IActionResultExamples.Controllers;

public class HomeController : Controller
{
    // GET
    [Route("/")]
    public IActionResult Index()
    {
        string content =
            "<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n    <meta charset=\"UTF-8\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    \n    <title>My Sample Page</title>\n</head>\n<body>\n\n    <header>\n        <h1>Welcome to My Website</h1>\n    </header>\n\n    <main>\n        <h2>About This Page</h2>\n        <p>This is a paragraph of text. It's the main way to display written content.</p>\n        \n        <p>This paragraph includes <strong>bold (important)</strong> text and <em>emphasized (italic)</em> text.</p>\n\n        <h2>My Hobbies</h2>\n        <ul>\n            <li>Reading</li>\n            <li>Hiking</li>\n            <li>Coding</li>\n        </ul>\n\n        <h2>More Information</h2>\n        <p>Here is a link to <a href=\"https://www.google.com\">Google</a>.</p>\n    </main>\n\n    <footer>\n        <p>&copy; 2025 Your Name Here</p>\n    </footer>\n\n</body>\n</html>";
        return new ContentResult()
        {
            Content = content,
            ContentType = "text/html",
            StatusCode = 200
        };
        
        // Equivalent shorthand but the class must inherit from Controller:
       //  return Content("Hello from HomeController.Index()", "text/plain");
    }
    
}