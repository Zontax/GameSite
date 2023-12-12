using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

public class LanguageRouteMiddleware
{
    readonly RequestDelegate _next;

    public LanguageRouteMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;
        var segments = path.Value?.Split('/').Where(s => !string.IsNullOrEmpty(s)).ToList();

        // Перевіряємо, чи перший сегмент в URL - код мови
        if (segments?.Count > 0 && IsLanguageCode(segments[0]))
        {
            var lang = segments[0];
            var remainingPath = string.Join("/", segments.Skip(1));

            // Перенаправляємо запит без мови в URL
            context.Request.Path = "/" + remainingPath;
            context.Response.Redirect($"/{lang}/{remainingPath}");
        }

        await _next(context);
    }

    bool IsLanguageCode(string code)
    {
        // Перевіряємо, чи код - це можливий код мови (наприклад, "en", "uk", тощо)
        List<string> supportedLanguages = new() { "en", "uk" };
        return supportedLanguages.Contains(code.ToLower());
    }
}

public static class LanguageRouteMiddlewareExtensions
{
    public static IApplicationBuilder UseLanguageRoutes(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LanguageRouteMiddleware>();
    }
}