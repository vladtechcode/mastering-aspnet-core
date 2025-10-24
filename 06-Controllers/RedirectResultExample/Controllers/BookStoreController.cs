using Microsoft.AspNetCore.Mvc;

namespace RedirectResultExample.Controllers;

public class BookStoreController : Controller
{
    // GET
    [Route("store/books")]
    public IActionResult Index()
    {
        return View();
    }
}