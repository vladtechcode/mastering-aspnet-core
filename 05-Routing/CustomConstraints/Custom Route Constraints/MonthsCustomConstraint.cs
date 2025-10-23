namespace CustomConstraints.Custom_Route_Constraints;

public class MonthsCustomConstraint: IRouteConstraint
{
    string [] months = new string[]
    {
        "january", "february", "march", "april", "may", "june", "july", "august", "september", "october",
        "november", "december"
    };
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values,
        RouteDirection routeDirection)
    {
         if(values.TryGetValue(routeKey, out var value) && value != null)
         {
             var parameterValue = value.ToString()?.ToLower();
             return months.Contains(parameterValue);
         } 
         return false;
    }
}