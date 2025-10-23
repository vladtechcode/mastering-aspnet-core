using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace IActionResultExamples.Controllers;

public class FileController : Controller
{
    private readonly IWebHostEnvironment _environment;
    public FileController(IWebHostEnvironment hostEnvironment)
    {
        this._environment = hostEnvironment;
    }
    // GET
    [Route("file-pdf")]
    public IActionResult Index()
    {
        // Return a virtual file result. "~/files/text-example.rtf" is a virtual path.
        return new VirtualFileResult("/files/sample.pdf", "application/pdf"); 
    }

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

    [Route("file-bytes")]
    public IActionResult GetFileBytes()
    {
        // Get the path to the PDF file
        string path = Path.Combine(_environment.ContentRootPath, "wwwroot/files/sample.pdf");
        // Read the file into a byte array
        var bytes =  System.IO.File.ReadAllBytes(path);
        // Return the file as a FileContentResult
        return new FileContentResult(bytes, "application/pdf");
    }
}