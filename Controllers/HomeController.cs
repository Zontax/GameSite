using System.Diagnostics;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Controllers;

public class HomeController : Controller
{
    ILogger<HomeController> logger;
    ApplicationDbContext context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
        var all = context.Publications.ToList();
        ViewBag.Publications = all;

        var commentsCount = context.Comments
        .GroupBy(com => com.PublicationId)
        .Select(group => new { PublicationId = group.Key, Count = group.Count() })
        .ToDictionary(i => i.PublicationId, i => i.Count);
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    public ActionResult News()
    {
        var news = context.Publications.ToList().Where(x => x.TypeId == Models.Type.Новина);
        ViewBag.Publications = news;

        var commentsCount = context.Comments
        .GroupBy(com => com.PublicationId)
        .Select(group => new { PublicationId = group.Key, Count = group.Count() })
        .ToDictionary(i => i.PublicationId, i => i.Count);
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    public ActionResult Reviews()
    {
        var reviews = context.Publications.ToList().Where(x => x.TypeId == Models.Type.Огляд); ;
        ViewBag.Publications = reviews;

        var commentsCount = context.Comments
        .GroupBy(com => com.PublicationId)
        .Select(group => new { PublicationId = group.Key, Count = group.Count() })
        .ToDictionary(i => i.PublicationId, i => i.Count);
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    public ActionResult Articles()
    {
        return View();
    }

    public ActionResult Guides()
    {
        return View();
    }

    public ActionResult Videos()
    {
        return View();
    }

    public ActionResult Podcasts()
    {
        return View();
    }

    [Authorize]
    public ActionResult AddPublication()
    {
        var typeList = Enum.GetValues(typeof(Models.Type));
        ViewBag.SelectItems = new SelectList(typeList);
        ViewBag.Username = User?.Identity?.Name ?? "null";
        ViewBag.IsAuthenticated = User?.Identity?.IsAuthenticated ?? false;

        return View();
    }

    [HttpPost]
    public ActionResult AddPublication(Publication publication)
    {
        if (string.IsNullOrEmpty(publication.Tags))
            ModelState.AddModelError(nameof(publication.Tags), "!!!");

        if (ModelState.IsValid)
        {
            context.Publications.Add(publication);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            var typeList = Enum.GetValues(typeof(Models.Type));
            ViewBag.SelectItems = new SelectList(typeList);
            return View(publication);
        }
    }

    public ActionResult Show(int id)
    {
        var publication = context.Publications.Find(id);
        var coments = context.Comments.Where(x => x.PublicationId == id).ToList();

        ViewBag.Publication = publication;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public ActionResult Show(int id, string author, string text)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(text))
            {
                Comment coment = new(id, author, text);
                context.Comments.Add(coment); context.SaveChanges();
                context.SaveChanges();
            }

            return RedirectToAction("Show", id);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    //! Error
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
