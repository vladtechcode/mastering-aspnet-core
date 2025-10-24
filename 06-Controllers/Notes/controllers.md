# Controllers

### The High-Level Analogy: A Restaurant Manager

Think of a controller as the **manager of a restaurant**.

* **The Customer (User):** Makes a request (e.g., "I want the menu," or "I'd like to order the steak, medium-rare").
* **The Host/Routing System:** Greets the customer and directs their request to the correct manager. In ASP.NET Core, this is the **Routing Middleware**.
* **The Manager (Controller):** Receives the request. They don't cook the food or wash the dishes themselves. Instead, they coordinate the work.
    * They might ask the **Kitchen (Model)** to prepare the steak.
    * They take the prepared dish.
    * They tell the **Waiter (View)** how to present the dish to the customer.
* **The Result (Response):** The customer gets their beautifully plated steak.

In short, the controller's job is to **receive a request, coordinate the necessary work, and decide what response to send back.**

-----

### The Technical Workflow: From Request to Response

Here’s the step-by-step process of how a controller works within the ASP.NET Core request pipeline:

#### 1\. The Request Arrives

A user sends an HTTP request to your application, for example, by typing `https://yourapp.com/Products/Details/5` into their browser.

#### 2\. Routing Determines the Target

ASP.NET Core's **Routing Middleware** examines the URL. By default, it uses a pattern like `/{controller}/{action}/{id?}`.

* `Products`: This part of the URL maps to a controller class named `ProductsController`. The framework follows a naming convention, looking for a class that ends with "Controller".
* `Details`: This maps to a public method inside `ProductsController` named `Details`. This method is called an **Action Method**.
* `5`: This is an optional parameter that will be passed to the `Details` method.

#### 3\. The Controller is Activated

The framework finds the `ProductsController` class and creates an instance of it to handle this specific request. If the controller has dependencies (like a database service), they are injected into the constructor at this stage (Dependency Injection).

#### 4\. The Action Method is Executed

The framework calls the `Details(int id)` method on the newly created controller instance, passing the value `5` as the `id` parameter.

```csharp
// Inside ProductsController.cs
public IActionResult Details(int id)
{
    // The value of 'id' here is 5
    // ... logic happens here ...
}
```

#### 5\. Inside the Action Method: The "Work"

This is where the core logic resides. The action method typically performs these steps:

* **Process Input:** It receives data from the URL (`id`), form submissions, or the request body.
* **Interact with the Model:** It communicates with the business logic and data layers (the "Model" in MVC). This could mean fetching data from a database, calling an external service, or performing calculations.
* **Make a Decision:** Based on the results from the model, it decides what to do next. For example, if the product with ID 5 was found, prepare to show its details. If it wasn't found, prepare to show a "Not Found" error.

#### 6\. Return a Result

The controller's final job is to produce a result. Action methods don't return raw HTML or JSON directly. Instead, they return an object that implements the `IActionResult` interface. This object is a set of instructions for the framework on what kind of response to generate.

Common return types include:

* `View()`: Tells the framework to find and render a Razor View file to produce an HTML page. You can pass data (a "view model") to the view.
* `Ok(data)`: Returns an HTTP 200 OK status with data (often serialized as JSON for APIs).
* `NotFound()`: Returns an HTTP 404 Not Found error.
* `BadRequest()`: Returns an HTTP 400 Bad Request error.
* `RedirectToAction("Index")`: Instructs the browser to redirect to another action method.

-----

### Code Example

Let's put it all together with a simple example.

**1. A simple Model:**

```csharp
// Models/Product.cs
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

**2. The Controller:**

- A controller is just a C\# class that inherits from `Microsoft.AspNetCore.Mvc.Controller`.
- The class name should be suffixed with "Controller".
- The [Controller] attribute is applied to the same class or to its base class.
- Optional
  - It is a public class.
  - Inherited from `Microsoft.AspNetCore.Mvc.Controller`.


```csharp
// Controllers/ProductsController.cs
using Microsoft.AspNetCore.Mvc;
// Assume we have a service to get product data
// using YourApp.Services; 
// using YourApp.Models;

[Controller]
public class ProductsController : Controller
{
    // This would typically be a database service, injected here
    // For simplicity, we'll use a fake list.
    private readonly List<Product> _products = new List<Product>
    {
        new Product { Id = 5, Name = "Laptop Pro", Price = 1200.00m },
        new Product { Id = 7, Name = "Wireless Mouse", Price = 45.00m }
    };

    // Handles GET request to /Products or /Products/Index
    public IActionResult Index()
    {
        // 1. Get all products from the "model" layer
        var allProducts = _products;

        // 2. Return a View() result, passing the list of products to it.
        // The framework will look for a file at /Views/Products/Index.cshtml
        return View(allProducts);
    }

    // Handles GET request to /Products/Details/5
    public IActionResult Details(int id)
    {
        // 1. Find the specific product from the "model" layer
        var product = _products.FirstOrDefault(p => p.Id == id);

        // 2. Decide what to do. If the product doesn't exist...
        if (product == null)
        {
            // Return an HTTP 404 Not Found response
            return NotFound();
        }

        // 3. If it exists, return a View() result, passing the single product.
        // The framework will look for a file at /Views/Products/Details.cshtml
        return View(product);
    }
}
```

### MVC Controllers vs. API Controllers

It's important to note this distinction:

* **MVC Controllers (like the one above):** Inherit from `Controller`. They are primarily used for building traditional web applications that serve HTML pages. They use `View()` to return results.
* **API Controllers:** Inherit from `ControllerBase` and are decorated with the `[ApiController]` attribute. They are used for building APIs that return data (like JSON or XML) to be consumed by client-side applications (like React, Angular, or mobile apps). They return results like `Ok(product)`, `CreatedAtAction(...)`, etc.

## Enable routing in controllers

### AddControllers()
- `builder.Services.AddControllers();`
- Adds all controllers as services in the IServiceCollection, so they can be accessed when a specific endpoint needs it.

### MapControllers()
- `app.MapControllers();`
- Adds all action methods as endpoints, so that, no need of using `UseEndpoints()` method for adding action methods as endpoints.

## Responsibilities of Controllers
- Reading requests
  - Extracting data values from request such as query string parameters, request body, request cookies, request headers, etc.
- Invoking models
  - Calling business logic methods.
  - Generally business operations are available as 'services'. 
- Validation 
  - Validate incoming request details (query string parameters, request body, request cookies, request headers etc.)
- Preparing Response
  - Choosing what kind of response has to be sent to teh client and also preparing the response (action result).