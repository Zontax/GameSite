using Microsoft.AspNetCore.Mvc;

namespace GameSite.Controllers;

public class LanguageController : Controller
{
    public IActionResult SetLanguage(string lang, string returnUrl)
    {
        Response.Cookies.Append("lang", lang);
        return LocalRedirect(returnUrl);
    }
}