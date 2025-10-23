# IActionResult

It is the parent interface for all action results in ASP.NET Core MVC. It defines a contract for classes that represent the result of an action method. When an action method returns an IActionResult, it indicates that the method will produce a response to be sent to the client.


## ContentResult

ContentResult can represent any type of response, based on the specified MIME type. Mime types represent type of the content such as text/plain, text/html, application/json, application/xml, and application/pdf etc.

```c#
public class HomeController : Controller
{
    // GET
    [Route("home")]
    [Route("/")]
    public IActionResult Index()
    {
        return new ContentResult()
        {
            Content = "Hello from HomeController.Index()",
            ContentType = "text/plain",
            StatusCode = 200
        };
        // Equivalent shorthand but the class must inherit from Controller:
       //  return Content("Hello from HomeController.Index()", "text/plain");
    }
    
    
}
```


## JsonResult

JsonResult is used to return JSON-formatted data. It serializes the provided object to JSON format.

 ```c#
using IActionResultExamples.Models;
using Microsoft.AspNetCore.Mvc;

namespace IActionResultExamples.Controllers;

public class PersonController : Controller
{
    // GET
    [Route("person")]
    public IActionResult Index()
    {
        Person person = new Person("John", "Doe", 30);
        string json = System.Text.Json.JsonSerializer.Serialize(person);
        
        return new JsonResult(person);
        
        // Equivalent shorthand but the class must inherit from Controller:
        // return Json(person);
        
        // Alternative using ContentResult:
        // return  Content(json, "application/json");
    }
}
 ```

## FileResult
FileResult is used to return files to the client. It can return files from the file system, byte arrays, or streams.

### VirtualFileResult
> - Represents a file from a virtual path.
> - Used when the file is part of the web application and needs to be sent as response
> - `return new VirtualFileResult("~/files/sample.pdf", "application/pdf");`
> - Equivalent shorthand but the class must inherit from Controller:
>  - `return VirtualFile("~/files/sample.pdf", "application/pdf");`

```c#
    [Route("file-pdf")]
    public IActionResult Index()
    {
        // Return a virtual file result. "~/files/text-example.rtf" is a virtual path.
        return new VirtualFileResult("/files/sample.pdf", "application/pdf"); 
    }
```

### PhysicalFileResult
> - Represents a file from the physical file system.
> - Used when the file is stored on disk and needs to be sent as response.
> - `return new PhysicalFileResult("C:\\files\\sample.pdf", "application/pdf");`
> - Equivalent shorthand but the class must inherit from Controller:
>   - `return PhysicalFile("C:\\files\\sample.pdf", "application/pdf");`

```c#
    [Route("file/static/{name}")]
    public IActionResult DownloadFile(string name)
    {
        // Construct the path to the static content directory
        string searchDirectory = Path.Combine(_environment.ContentRootPath, "static");
 
        // Check if the directory exists
        if (!Directory.Exists(searchDirectory))
        {
            // Handle the case where the directory does not exist
            return NotFound("Static content directory not found.");
        }
        
        // Search for the file with the given name and any extension
        string searchPattern = $"{name}.*";
        // Find the first matching file
        string? foundFilePath = Directory.EnumerateFiles(searchDirectory, searchPattern).FirstOrDefault();
        // If no file is found, return a 404 Not Found response
        if (foundFilePath == null)
            return NotFound("File not found");
        
        // Determine the MIME type based on the file extension
        var provider = new FileExtensionContentTypeProvider();
        // Try to get the content type
        if (!provider.TryGetContentType(foundFilePath, out var contentType))
        {
            // If the MIME type can't be determined, provide a default one.
            contentType = "application/octet-stream";
        }
 
        // Return the file as a PhysicalFileResult
        return new PhysicalFileResult(foundFilePath, contentType);
         
    }
```

### FileContentResult
> - Represents a file from the byte []
> - Used when part of the file or byte [] from other data source has to be sent as response.
> - `return new FileContentResult(fileBytes, "application/pdf") { FileDownloadName = "sample.pdf" };`
> - Equivalent shorthand but the class must inherit from Controller:
>   - `return File(fileBytes, "application/pdf", "sample.pdf");`

```c#
    [Route("file-bytes")]
    public IActionResult GetFileBytes()
    {
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot/files/sample.pdf");
        var bytes =  System.IO.File.ReadAllBytes(path);
        return new FileContentResult(bytes, "application/pdf");
    }```

- IActionResult (Interface)
  - ActionResult (Abstract Base Class)
    
    # Status Code Results (No Body)
    - StatusCodeResult
      - OkResult (200 OK)
      - NoContentResult (204 No Content)
      - BadRequestResult (400 Bad Request)
      - UnauthorizedResult (401 Unauthorized)
      - NotFoundResult (404 Not Found)
      - ConflictResult (409 Conflict)
      - UnprocessableEntityResult (422 Unprocessable Entity)
      - UnsupportedMediaTypeResult (415 Unsupported Media Type)
    
    # Object Results (Status Code + Body)
    - ObjectResult
      - OkObjectResult (200 OK)
      - CreatedResult (201 Created)
      - CreatedAtActionResult (201 Created)
      - CreatedAtRouteResult (201 Created)
      - AcceptedResult (202 Accepted)
      - AcceptedAtActionResult (202 Accepted)
      - AcceptedAtRouteResult (202 Accepted)
      - BadRequestObjectResult (400 Bad Request)
      - NotFoundObjectResult (404 Not Found)
      - ConflictObjectResult (409 Conflict)
      - UnprocessableEntityObjectResult (422 Unprocessable Entity)

    # File Results
    - FileResult (Abstract Base Class)
      - FileContentResult (From byte array)
      - FileStreamResult (From a Stream)
      - VirtualFileResult (From a virtual path)
      - PhysicalFileResult (From a physical disk path)
      
    # Redirect Results
    - RedirectResult (302 Found)
    - LocalRedirectResult (302 Found, local URLs only)
    - RedirectToActionResult (302 Found, to a controller action)
    - RedirectToRouteResult (302 Found, to a route name)
    - RedirectToPageResult (302 Found, to a Razor Page)
    
    # View-Based Results
    - ViewResult (Renders a full view)
    - PartialViewResult (Renders a partial view)
    - ViewComponentResult (Renders a view component)
    
    # Content-Specific Results
    - ContentResult (Returns a plain string)
    - JsonResult (Returns a JSON-serialized object)

    # Authentication Results
    - ChallengeResult (Triggers an auth challenge, e.g., 401/403)
    - ForbidResult (Triggers a 403 Forbidden)
    - SignInResult (Signs in the user)
    - SignOutResult (Signs out the user)
      
    # Other Results
    - EmptyResult (Returns no response body)