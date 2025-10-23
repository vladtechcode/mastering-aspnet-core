var builder = WebApplication.CreateBuilder(args);

/*
 * Add services to the container. It registers all the services that API controllers need to work, such as handling JSON serialization,
 * model binding, validation, and routing. Without this line, the application won't be able to process HTTP requests to the controllers.
 *
 * builder.Services.AddControllersWithViews();
 * Use this line instead if you are building an MVC application with views (Razor pages).
 * 
 */
builder.Services.AddControllers();

var app = builder.Build();
 
/*
 * This is the Routing step. This method looks at all the API controllers that were registered earlier and sets up the
 * routes for them. It specifically looks for routing attributes on your controller classes and methods, such as
 * [Route("api/[controller]")] and [HttpGet], [HttpPost], etc. Without this line, your application would have no
 * idea which controller or method to execute when a request like GET /api/products comes in.
 * This line creates that mapping.
 */
app.MapControllers();

app.Run();