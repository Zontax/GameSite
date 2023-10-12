﻿using System.Diagnostics;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Controllers;

public class HomeController : Controller
{
    readonly ILogger<HomeController> logger;
    readonly ApplicationDbContext context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        this.logger = logger;
        this.context = context;
    }

    public ActionResult Index()
    {
        var news = context.News.ToList().Skip(1);
        ViewBag.Publications = news;
        return View();
    }

    public ActionResult News()
    {
        var news = context.News.ToList().Skip(1);
        ViewBag.Publications = news;
        return View();
    }

    public string Reviews()
    {
        return "Пизда";
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

    [HttpGet]
    public ActionResult Show(int id)
    {
        var @new = context.News.ToList()[id - 1];
        var coments = context.NewsComments.Where(x => x.NewsId == id - 1).ToList();
        ViewBag.Publication = @new;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Show(int id, string author, string text)
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
    public ActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
