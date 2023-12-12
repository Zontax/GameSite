using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace GameSite.Data;

public class LanguageMiddleware
{
    readonly RequestDelegate _next;

    public LanguageMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var cookieLang = context.Request.Cookies["lang"];
        var userLanguage = string.IsNullOrEmpty(cookieLang) ? "uk" : cookieLang;

        CultureInfo.CurrentCulture = new CultureInfo(userLanguage);
        CultureInfo.CurrentUICulture = new CultureInfo(userLanguage);

        await _next(context);
    }
}