using System.Diagnostics;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameSite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var publications = _context.Publications.ToList().Skip(1);
        ViewBag.Publications = publications;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Show(int id)
    {
        var publication = _context.Publications.ToList()[id - 1];
        var coments = _context.Coments.Where(x => x.PublicationId == id - 1).ToList();
        ViewBag.Publication = publication;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Show(int id, string author, string text)
    {
        if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(text))
        {
            Coment coment = new(id - 1, author, text);
            _context.Coments.Add(coment);
            await _context.SaveChangesAsync();

            // Після успішного додавання коментаря редіректимо на ту саму сторінку
            return RedirectToAction("Show", new { id = id });
        }

        // Якщо дані недійсні, залишаємо користувача на тій же сторінці.
        var publication = _context.Publications.FirstOrDefault(p => p.Id == id);
        ViewBag.Publication = publication;
        var coments = _context.Coments.Where(x => x.PublicationId == id).ToList();
        ViewBag.Coments = coments;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
