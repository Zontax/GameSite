using System.Diagnostics;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly ApplicationDbContext context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public IActionResult Index()
    {
        var news = context.News.ToList().Skip(1);
        ViewBag.Publications = news;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Show(int id)
    {
        var @new = context.News.ToList()[id - 1];
        var coments = context.NewsComments.Where(x => x.NewsId == id - 1).ToList();
        ViewBag.Publication = @new;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Show(int id, string author, string text)
    {
        if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(text))
        {
            NewsComment coment = new(id - 1, author, text);
            context.NewsComments.Add(coment);
            await context.SaveChangesAsync();

            // Після успішного додавання коментаря редіректимо на ту саму сторінку
            return RedirectToAction("Show", new { id = id });
        }

        // Якщо дані недійсні, залишаємо користувача на тій же сторінці.
        var news = await context.News.FirstOrDefaultAsync(p => p.Id == id);
        var coments = await context.NewsComments.Where(x => x.NewsId == id).ToListAsync();
        ViewBag.Publication = news;
        ViewBag.Coments = coments;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
