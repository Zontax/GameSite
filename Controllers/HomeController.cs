using System.Diagnostics;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    public async Task<ActionResult> Index()
    {
        var all = await context.Posts.OrderByDescending(post => post.Date).ToListAsync();
        var commentsCount = await context.Comments
        .GroupBy(com => com.PostId)
        .Select(group => new { PostId = group.Key, Count = group.Count() })
        .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Posts = all;
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    public async Task<ActionResult> Tag(string tag)
    {
        var postsWithTag = await context.Posts
        .Where(post => post.Tags.Contains(tag))
        .ToListAsync();

        var commentsCount = await context.Comments
        .GroupBy(com => com.PostId)
        .Select(group => new { PostId = group.Key, Count = group.Count() })
        .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Posts = postsWithTag;
        ViewBag.CommentsCount = commentsCount;

        if (postsWithTag == null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public async Task<ActionResult> News()
    {
        var news = await context.Posts
        .Where(x => x.TypeId == Models.Type.Новина)
        .OrderByDescending(post => post.Date)
        .ToListAsync();
        var commentsCount = await context.Comments
        .GroupBy(com => com.PostId)
        .Select(group => new { PostId = group.Key, Count = group.Count() })
        .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Posts = news;
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    public async Task<ActionResult> Reviews()
    {
        var reviews = await context.Posts
        .Where(x => x.TypeId == Models.Type.Огляд)
        .OrderByDescending(post => post.Date)
        .ToListAsync();
        var commentsCount = await context.Comments
        .GroupBy(com => com.PostId)
        .Select(group => new { PostId = group.Key, Count = group.Count() })
        .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Posts = reviews;
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

    //[Authorize]
    public ActionResult Create()
    {
        var typeList = Enum.GetValues(typeof(Models.Type));
        ViewBag.SelectItems = new SelectList(typeList);

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Post post)
    {
        // if (string.IsNullOrEmpty(post.Tags))
        //     ModelState.AddModelError(nameof(post.Tags), "!!!");

        if (ModelState.IsValid)
        {
            context.Posts.Attach(post);
            context.Entry(post).State = EntityState.Added;
            //context.Posts.Add(post);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            var typeList = Enum.GetValues(typeof(Models.Type));
            ViewBag.SelectItems = new SelectList(typeList);
            return View(post);
        }
    }

    public async Task<ActionResult> Show(int id)
    {
        var post = await context.Posts.FindAsync(id);
        var coments = await context.Comments.Where(x => x.PostId == id).ToListAsync();

        ViewBag.Post = post;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Show(int id, string author, string text)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(text))
            {
                Comment coment = new(id, author, text);
                await context.Comments.AddAsync(coment);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Show", id);
        }
        else
        {
            return RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        var typeList = Enum.GetValues(typeof(Models.Type));
        ViewBag.SelectItems = new SelectList(typeList);

        Post post = await context.Posts.FindAsync(id);

        if (post != null)
        {
            return View(post);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Post post)
    {
        if (ModelState.IsValid)
        {
            context.Posts.Attach(post);
            // Встановити стан об'єкта як Modified, щоб EF знав, що цей об'єкт потрібно оновити в БД
            context.Entry(post).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        else
        {
            var typeList = Enum.GetValues(typeof(Models.Type));
            ViewBag.SelectItems = new SelectList(typeList);
            return View(post);
        }
    }

    //[Authorize]
    [HttpPost]
    public async Task<ActionResult> Delete(int id)
    {
        var post = await context.Posts.FindAsync(id);

        if (post != null)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }

    //! Error Action
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
