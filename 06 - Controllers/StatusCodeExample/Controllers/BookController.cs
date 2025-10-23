using Microsoft.AspNetCore.Mvc;

namespace StatusCodeExample.Controllers;

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
            return BadRequest("Book is not supplied");
            // return new BadRequestResult();
        }

        if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
        {
            return new BadRequestResult();
            // Alternative way to return BadRequest with a message
            // return BadRequest("Bookid cannot be null or empty");
        }

        int bookid = Convert.ToInt32(Request.Query["bookid"]);
        if (bookid <= 0)
        {
            return NotFound("Bookid must be greater than zero");
            // This is another way of returning NotFound
            // return new NotFoundResult();
        }

        if (bookid > 1000)
        {
            return NotFound("Bookid must be less than 1000");
        }

        if (!Convert.ToBoolean(Request.Query["isloggedin"]))
        {
            // Another way of returning Unauthorized
            // return new UnauthorizedResult();
            return Unauthorized("Please login to access the book");
        }

        return new VirtualFileResult("~/files/sample.pdf", "application/pdf");
    }
}