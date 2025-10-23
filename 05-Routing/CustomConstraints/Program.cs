using CustomConstraints.Custom_Route_Constraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint)));
var app = builder.Build();

app.Map("sales-report/{year:int}/{month:months}", async (context) =>
{
    int year = Convert.ToInt32(context.Request.RouteValues["year"]!);
    string month = (string)context.Request.RouteValues["month"]!;
    await context.Response.WriteAsync($"sales-report - {year}/{month}");
});

app.MapFallback(async (context) =>
{
    await context.Response.WriteAsync("No such report");
});

app.Run();