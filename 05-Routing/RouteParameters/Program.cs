var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Eg: files/sample.text
app.Map ("files/{filename}.{extension}", async (context) => {

    string filename = context.Request.RouteValues["filename"]?.ToString() ?? "No filename";
    string extension = context.Request.RouteValues["extension"]?.ToString() ?? "No extension";
    await context.Response.WriteAsync($"Files endpoint: name:{filename} and extension: {extension}");
});

/*
 * Route Parameter with default value
 * 
 * You define a default value directly within the route template
 * by adding an equals sign = and the desired value after the
 * parameter name.
 * 
 * Syntax: {parameterName=defaultValue}
 * This tells the routing engine two things:
 * 
 * If a value for parameterName is present in the URL, use it.
 * If a value for parameterName is missing from the URL,
 * use defaultValue instead and still consider 
 * the route a match.
 * 
 */

//Eg: employee/profile/john
// Default parameter for employeename is "John Doe"
app.Map("employee/profile/{employeename=John Doe}",async (context) =>
{
    string employeeName = Convert.ToString(context.Request.RouteValues["employeename"]) ?? "No employee name";
    await context.Response.WriteAsync($"Employee endpoint: {employeeName}");
});

/*
 * Optional Route Parameters
 * 
 * You define an optional parameter by adding a question mark ? after the parameter name.
 * 
 * Syntax: {parameterName?}
 * This tells the routing engine two things:
 * 
 * If a value for parameterName is present in the URL, use it.
 * If a value for parameterName is missing from the URL,
 * consider the route a match and assign a null value to parameterName.
 * 
 */

// Eg: product/details/1
// Optional parameter for id
app.Map("products/details/{id?}", async(context) =>
{
    
    if (context.Request.RouteValues.ContainsKey("id"))
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product details endpoint: {id}");
    }
    else
    {
        await context.Response.WriteAsync("Product details endpoint: id was not supplied");
    }
    
});

app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("Fallback endpoint");
});

app.Run();
