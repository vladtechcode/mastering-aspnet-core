var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Map("files/{filename}.{extension}", async (context) => {

    string filename = context.Request.RouteValues["filename"]?.ToString() ?? "No filename";
    string extension = context.Request.RouteValues["extension"]?.ToString() ?? "No extension";
    await context.Response.WriteAsync($"Files endpoint: name:{filename} and extension: {extension}");
});

//Eg: employee/profile/John
app.Map("employee/profile/{EmployeeName:length(4,7):alpha=John-Doe}", async (context) =>
{
    string employeeName = Convert.ToString(context.Request.RouteValues["EmployeeName"]) ?? "No employee name";
    await context.Response.WriteAsync($"Employee endpoint: {employeeName}");
});

app.Map("products/details/{id:int:range(1,1000)?}", async (context) =>
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

app.Map("daily-digest-report/{reportdate:datetime}", async (HttpContext context) => 
{
    DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
    await context.Response.WriteAsync($"Daily digest report for: {reportDate.ToShortDateString()}");
});

//Eg: cities/{cityid}
app.Map("cities/{cityid:guid}", async (context) => {

    Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
    await context.Response.WriteAsync($"Cities information - {cityId}");
});

//Eg: sales-report/2030/apr
app.Map("sales-report/{year:int:min(1990)}/{month:regex(^(apr|jul|oct)$)}", async (context) => {
     
    int year = Convert.ToInt32(context.Request.RouteValues["year"]);
    string? month = Convert.ToString(context.Request.RouteValues["month"]);
    await context.Response.WriteAsync($"Sales report - {month}-{year}");

});

app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync($"No route match at {context.Request.Path}");
});

app.Run();
