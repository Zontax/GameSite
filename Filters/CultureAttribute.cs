using System.Globalization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GameSite;

public class CultureAttribute : ActionFilterAttribute, IActionFilter
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var langCookie = context.HttpContext.Request.Cookies["lang"];
        string cultureName = !string.IsNullOrEmpty(langCookie) ? langCookie : "en";

        List<string> cultures = new() { "uk", "en" };

        if (!cultures.Contains(cultureName))
        {
            cultureName = "en";
        }

        CultureInfo cultureInfo = new(cultureName);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Not implemented
    }
}