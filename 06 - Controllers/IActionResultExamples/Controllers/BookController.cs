using Microsoft.AspNetCore.Mvc;

namespace IActionResultExamples.Controllers;

/* BookController demonstrates various IActionResult responses based on query parameters.
 * It checks for the presence and validity of 'bookid' and 'isloggedin' parameters
 * and returns appropriate responses including error messages and a PDF file.
 * This example illustrates how to handle different request scenarios in an ASP.NET Core MVC controller without using model binding.
 * Model binding is intentionally avoided to showcase direct query parameter handling.
 * However, in a real-world application, model binding is recommended for cleaner code and better maintainability.
 */
public class BookController : Controller
{
    // GET
    [Route("book")]
    public IActionResult Index()
    {
        if (!Request.Query.ContainsKey("bookid"))
        {
            Response.StatusCode = 400;
            return Content("Book is not supplied");
        }

        if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
        {
            Response.StatusCode = 400;
            return Content("Please provide a bookid");
        }

        int bookid = Convert.ToInt32(Request.Query["bookid"]);
        if (bookid <= 0)
        {
            Response.StatusCode = 400;
            return Content("Bookid must be greater than zero");
        }

        if (bookid > 1000)
        {
            Response.StatusCode = 400;
            return Content("Bookid must be less than or equal to 1000");
        }

        if (!Convert.ToBoolean(Request.Query["isloggedin"]))
        {
            return new ContentResult()
            {
                Content = "Please login to access the book",
                ContentType = "text/plain",
                StatusCode = 401
            };
        }

        return new VirtualFileResult("~/files/sample.pdf", "application/pdf");
    }
}