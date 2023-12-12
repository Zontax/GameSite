using Microsoft.AspNetCore.Mvc;

namespace GameSite.Controllers;

public class LanguageController : Controller
{
    public IActionResult SetLanguage(string lang, string returnUrl)
    {
        Response.Cookies.Append("lang", lang);
        return LocalRedirect(returnUrl);
    }

    // public IActionResult ChangeCulture(string lang)
    // {
    //     string returnUrl = Request.Headers["Referer"].ToString();
    //     List<string> cultures = new() { "uk", "en" };

    //     if (!cultures.Contains(lang))
    //     {
    //         lang = "uk";
    //     }

    //     Response.Cookies.Append("lang", lang, new CookieOptions
    //     {
    //         HttpOnly = false,
    //         Expires = DateTime.Now.AddYears(1)
    //     });

    //     return RedirectToAction(nameof(HomeController.Index));
    // }
}