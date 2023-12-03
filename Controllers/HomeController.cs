using System.Diagnostics;
using System.Text.RegularExpressions;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameSite.Controllers;

public class HomeController : Controller
{
    readonly ILogger<HomeController> logger;
    readonly ApplicationDbContext context;
    readonly IWebHostEnvironment webHostEnv;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnv)
    {
        this.logger = logger;
        this.context = context;
        this.webHostEnv = webHostEnv;
    }

    public async Task<ActionResult> Index()
    {
        var posts = await context.Posts
            .OrderByDescending(post => post.Date)
            .ToListAsync();

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    public IActionResult UserList()
    {
        var users = context.ApplicationUsers.ToList();

        return View(users);
    }

    public async Task<ActionResult> Tag(string tag)
    {
        string[] tags = tag.Split(new[] { ',' });
        var searchTags = string.Join("|", tags.Select(t => $"\\b{Regex.Escape(t.Trim())}\\b"));
        List<Post> postsWithTag = await context.Posts
            .Where(post => Regex.IsMatch(post.Tags, searchTags))
            .ToListAsync();

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Tag = tag;
        ViewBag.Posts = postsWithTag;
        ViewBag.CommentsCount = commentsCount;

        if (postsWithTag == null) return RedirectToAction(nameof(Index));

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

    public async Task<ActionResult> Articles()
    {
        var reviews = await context.Posts
            .Where(x => x.TypeId == Models.Type.Стаття)
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

    public async Task<ActionResult> Guides()
    {
        var reviews = await context.Posts
            .Where(x => x.TypeId == Models.Type.Гайд)
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

    public IActionResult Videos()
    {
        return View();
    }

    public ActionResult Podcasts()
    {
        return View();
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

            return RedirectToAction(nameof(Show), id);
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public ActionResult Create()
    {
        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.Type)));
        return View();
    }

    [HttpPost, Authorize]
    public async Task<ActionResult> Create(Post post, IFormFile file)
    {
        // if (string.IsNullOrEmpty(post.ImageUrl))
        //     ModelState.AddModelError(nameof(post.ImageUrl), "Де файл?!");

        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                // Збережіть файл у папці wwwroot або в потрібному вам шляху
                var uploadsDirectory = Path.Combine(webHostEnv.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsDirectory)) Directory.CreateDirectory(uploadsDirectory);

                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(webHostEnv.WebRootPath, "uploads", uniqueFileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                post.ImageUrl = "/post_title_image/" + uniqueFileName;
            }

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.Type)));
        return View(post);
    }

    [Authorize]
    public async Task<ActionResult> Edit(int id)
    {
        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.Type)));
        var post = await context.Posts.FindAsync(id);

        if (post == null) return NotFound();
        return View(await context.Posts.FindAsync(id));
    }

    [HttpPost, Authorize]
    public async Task<ActionResult> Edit(Post post, IFormFile file)
    {
        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                string uploadsDirectory = Path.Combine(webHostEnv.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsDirectory)) Directory.CreateDirectory(uploadsDirectory);

                string fileExtension = Path.GetExtension(file.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(webHostEnv.WebRootPath, "uploads", uniqueFileName);

                using (FileStream fileStream = new(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                post.ImageUrl = "/post_title_image/" + uniqueFileName;
            }
            else
            {
                // Якщо файл не був вибраний, використовуйте поточне зображення
                var existingPost = await context.Posts.FindAsync(post.Id);
                post.ImageUrl = existingPost?.ImageUrl;
            }

            context.Posts.Update(post);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.Type)));
        post.ImageUrl = context.Posts.Find(post.Id)?.ImageUrl;
        await context.SaveChangesAsync();
        return View(post);
    }

    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var post = await context.Posts.FindAsync(id);

        if (post == null) return NotFound();

        context.Posts.Remove(post);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<ActionResult> SetRole()
    {
        await context.SaveChangesAsync();
        return View();
    }

    [HttpPost, Authorize]
    public async Task<ActionResult> SetRole(int userId)
    {
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
