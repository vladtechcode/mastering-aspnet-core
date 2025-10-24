using Microsoft.AspNetCore.Mvc;

namespace RedirectResultExample.Controllers;

public class BooksController : Controller
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

        
        return new RedirectToActionResult("Index", "BookStore", new {});
    }
}