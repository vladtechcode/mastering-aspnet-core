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