
## **1. Understanding HTTP Redirects**

When you redirect, the server sends a response telling the browser to go to a different URL. The main types are:

- **302 (Temporary)**: "This page moved temporarily, keep using the old URL"
- **301 (Permanent)**: "This page moved forever, update your bookmarks"
- **307/308**: Similar to 302/301 but preserve the HTTP method (GET, POST, etc.)

## **2. Redirecting in Controllers**

### **Basic Redirect Methods**

```csharp
public class RedirectController : Controller
{
    // Temporary redirect (302) - most common
    public IActionResult TemporaryRedirect()
    {
        return Redirect("/home/index");
    }
    
    // Permanent redirect (301) - for SEO and moved pages
    public IActionResult PermanentRedirect()
    {
        return RedirectPermanent("/home/index");
    }
    
    // Preserve HTTP method (307)
    public IActionResult PreserveMethod()
    {
        return RedirectPreserveMethod("/home/index");
    }
    
    // Permanent + preserve method (308)
    public IActionResult PermanentPreserveMethod()
    {
        return RedirectPermanentPreserveMethod("/home/index");
    }
}
```

### **RedirectToAction - Type-Safe Redirects**

This is the **most common and recommended** approach because it's type-safe and doesn't break if you rename actions:

```csharp
public class ProductController : Controller
{
    // Redirect to action in same controller
    public IActionResult Create()
    {
        // ... save product logic
        return RedirectToAction("Index"); // Goes to Index() in ProductController
    }
    
    // Redirect to action in different controller
    public IActionResult GoToUserProfile()
    {
        return RedirectToAction("Profile", "User");
        // Goes to Profile() in UserController
    }
    
    // Redirect with route parameters
    public IActionResult ViewProduct(int id)
    {
        return RedirectToAction("Details", new { id = 5 });
        // Goes to Details(int id) with id=5
    }
    
    // Redirect with multiple parameters
    public IActionResult Search()
    {
        return RedirectToAction("Results", "Search", new 
        { 
            query = "laptops", 
            page = 1,
            sortBy = "price" 
        });
        // Creates URL: /Search/Results?query=laptops&page=1&sortBy=price
    }
    
    // Permanent redirect to action
    public IActionResult OldProduct()
    {
        return RedirectToActionPermanent("NewProduct");
    }
}
```

### **RedirectToRoute - Using Named Routes**

```csharp
public class OrderController : Controller
{
    public IActionResult PlaceOrder()
    {
        // ... order logic
        
        // Redirect to a named route
        return RedirectToRoute("OrderConfirmation", new { orderId = 12345 });
    }
    
    public IActionResult PermanentRouteRedirect()
    {
        return RedirectToRoutePermanent("HomePage");
    }
}
```

You define named routes in `Program.cs`:

```csharp
app.MapControllerRoute(
    name: "OrderConfirmation",
    pattern: "orders/{orderId}/confirmation",
    defaults: new { controller = "Order", action = "Confirmation" });
```

### **RedirectToPage - For Razor Pages**

```csharp
public class LoginModel : PageModel
{
    public IActionResult OnPost()
    {
        // ... authentication logic
        
        // Redirect to another Razor Page
        return RedirectToPage("/Account/Profile");
    }
    
    // With parameters
    public IActionResult OnPostWithParam()
    {
        return RedirectToPage("/Products/Details", new { id = 10 });
    }
}
```

### **LocalRedirect - Security Best Practice**

**Always use LocalRedirect** when redirecting to URLs that come from user input to prevent **open redirect vulnerabilities**:

```csharp
public class AccountController : Controller
{
    public IActionResult Login(string returnUrl)
    {
        // BAD - Vulnerable to open redirects
        // return Redirect(returnUrl); // User could pass "http://evil.com"
        
        // GOOD - Only allows local URLs
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        
        return RedirectToAction("Index", "Home");
    }
    
    // LocalRedirectPermanent also exists
    public IActionResult SecureRedirect(string url)
    {
        return LocalRedirectPermanent(url);
    }
}
```

## **3. Middleware-Based Redirects**

### **Simple Redirect Middleware**

In `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Custom redirect logic
app.Use(async (context, next) =>
{
    // Redirect all HTTP to HTTPS
    if (!context.Request.IsHttps)
    {
        var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        context.Response.Redirect(httpsUrl, permanent: true);
        return;
    }
    
    // Redirect specific paths
    if (context.Request.Path == "/old-blog")
    {
        context.Response.Redirect("/blog", permanent: true);
        return;
    }
    
    await next();
});

app.MapControllers();
app.Run();
```

### **URL Rewriting Middleware - Most Powerful**

```csharp
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var rewriteOptions = new RewriteOptions()
    // Simple redirects
    .AddRedirect("old-page", "new-page", 301)
    .AddRedirect("products/(.*)", "shop/$1", 301) // Regex redirect
    
    // HTTPS redirect
    .AddRedirectToHttpsPermanent()
    
    // WWW redirect
    .AddRedirectToWwwPermanent()
    
    // Rewrite (internal, no redirect to browser)
    .AddRewrite(@"^blog/(\d+)", "blog/post?id=$1", skipRemainingRules: false)
    
    // Custom rule
    .Add(context =>
    {
        var request = context.HttpContext.Request;
        
        // Redirect mobile users to mobile site
        if (request.Headers.UserAgent.ToString().Contains("Mobile"))
        {
            context.Result = RuleResult.EndResponse;
            context.HttpContext.Response.Redirect("https://m.example.com");
        }
    });

app.UseRewriter(rewriteOptions);
app.MapControllers();
app.Run();
```

### **Apache mod_rewrite Rules (If Migrating)**

You can even load Apache-style rules from a file:

```csharp
var options = new RewriteOptions()
    .AddApacheModRewrite(File.OpenText("ApacheModRewrite.txt"));

app.UseRewriter(options);
```

`ApacheModRewrite.txt`:
```
RewriteRule ^old-product/(\d+) /products/details/$1 [R=301,L]
RewriteRule ^blog$ /articles [R=302,L]
```

## **4. Redirect Based on Conditions**

```csharp
public class ConditionalController : Controller
{
    public IActionResult SmartRedirect(int userId)
    {
        // Redirect based on user status
        var user = GetUser(userId);
        
        if (!user.IsActive)
        {
            return RedirectToAction("Suspended", "Account");
        }
        
        if (user.IsAdmin)
        {
            return RedirectToAction("Dashboard", "Admin");
        }
        
        return RedirectToAction("Profile", "User", new { id = userId });
    }
    
    public IActionResult RedirectWithData(string category)
    {
        // Use TempData to pass data through redirect
        TempData["Message"] = "Product saved successfully!";
        TempData["Category"] = category;
        
        return RedirectToAction("Index", "Products");
    }
}
```

## **5. Redirecting with Query Strings**

```csharp
public class SearchController : Controller
{
    public IActionResult AdvancedSearch(string query, int page, string sortBy)
    {
        // All parameters automatically become query string
        return RedirectToAction("Results", new 
        { 
            q = query,           // ?q=laptops
            page = page,         // &page=1
            sort = sortBy        // &sort=price
        });
        // Result: /Search/Results?q=laptops&page=1&sort=price
    }
    
    // Preserve existing query string
    public IActionResult PreserveQuery()
    {
        var existingQuery = Request.QueryString;
        return Redirect($"/new-path{existingQuery}");
    }
}
```

## **6. Post-Redirect-Get Pattern (PRG)**

This prevents duplicate form submissions:

```csharp
public class FormController : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(ProductModel model)
    {
        if (!ModelState.IsValid)
        {
            // Show form again with errors
            return View(model);
        }
        
        // Save the product
        var productId = _productService.Save(model);
        
        // Redirect after POST (PRG pattern)
        // This prevents duplicate submissions if user refreshes
        TempData["SuccessMessage"] = "Product created successfully!";
        return RedirectToAction("Details", new { id = productId });
    }
    
    [HttpGet]
    public IActionResult Details(int id)
    {
        var product = _productService.GetById(id);
        return View(product);
    }
}
```

## **7. External URL Redirects**

```csharp
public class ExternalController : Controller
{
    // Redirect to external site
    public IActionResult GoToGoogle()
    {
        return Redirect("https://www.google.com");
    }
    
    // Build dynamic external URL
    public IActionResult ShareOnTwitter(string text)
    {
        var encodedText = Uri.EscapeDataString(text);
        var twitterUrl = $"https://twitter.com/intent/tweet?text={encodedText}";
        return Redirect(twitterUrl);
    }
    
    // Permanent external redirect (rare)
    public IActionResult OldDomain()
    {
        return RedirectPermanent("https://www.newdomain.com");
    }
}
```

## **8. Return URL Pattern (Common in Login)**

```csharp
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    
    [HttpPost]
    public IActionResult Login(LoginModel model, string returnUrl = null)
    {
        if (ModelState.IsValid && ValidateCredentials(model))
        {
            // Authenticate user...
            
            // Redirect back to where they came from
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            
            // Default redirect if no return URL
            return RedirectToAction("Index", "Home");
        }
        
        return View(model);
    }
}
```

## **9. Global Redirect Rules**

Create a custom middleware for complex redirect logic:

```csharp
public class RedirectMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string, string> _redirects;
    
    public RedirectMiddleware(RequestDelegate next)
    {
        _next = next;
        
        // Load redirects from database or configuration
        _redirects = new Dictionary<string, string>
        {
            { "/old-url-1", "/new-url-1" },
            { "/old-url-2", "/new-url-2" }
        };
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        
        if (_redirects.TryGetValue(path, out var newPath))
        {
            context.Response.Redirect(newPath, permanent: true);
            return;
        }
        
        await _next(context);
    }
}

// Register in Program.cs
app.UseMiddleware<RedirectMiddleware>();
```

## **10. Common Scenarios & Best Practices**

### **After Form Submission**
```csharp
[HttpPost]
public IActionResult Submit(MyModel model)
{
    // Save data
    _repository.Save(model);
    
    // Always redirect after successful POST (PRG pattern)
    return RedirectToAction("Success");
}
```

### **SEO-Friendly Redirects**
```csharp
// Old URL structure changed
public IActionResult OldProductUrl(int id)
{
    // Use 301 for SEO
    return RedirectToActionPermanent("Details", new { id, slug = "product-name" });
}
```

### **Localization Redirects**
```csharp
public IActionResult Index()
{
    var userCulture = GetUserCulture();
    return RedirectToAction("Index", new { culture = userCulture });
}
```

### **Error Handling**
```csharp
public IActionResult ProcessPayment(int orderId)
{
    try
    {
        _paymentService.Process(orderId);
        return RedirectToAction("Success");
    }
    catch (Exception ex)
    {
        TempData["Error"] = ex.Message;
        return RedirectToAction("PaymentError");
    }
}
```

## **Quick Reference Cheat Sheet**

```csharp
// Same controller
return RedirectToAction("ActionName");

// Different controller
return RedirectToAction("ActionName", "ControllerName");

// With parameters
return RedirectToAction("ActionName", new { id = 1, name = "test" });

// Permanent redirect
return RedirectToActionPermanent("ActionName");

// Named route
return RedirectToRoute("RouteName", new { id = 1 });

// Razor Page
return RedirectToPage("/PageName");

// External or absolute URL
return Redirect("https://example.com");

// Secure local redirect (user input)
return LocalRedirect(returnUrl);

// Manual response redirect
Response.Redirect("/path", permanent: true);
```

**Key Takeaways:**
- Use `RedirectToAction` for most internal redirects (type-safe)
- Use `LocalRedirect` for user-supplied URLs (security)
- Use 301 permanent for moved pages (SEO)
- Use 302 temporary for conditional redirects
- Always use Post-Redirect-Get pattern after form submissions
- Use URL Rewriting middleware for complex redirect rules
