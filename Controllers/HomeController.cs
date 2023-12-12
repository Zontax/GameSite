using System.Diagnostics;
using System.Text.RegularExpressions;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PagedList.Core;
//using Newtonsoft.Json;

namespace GameSite.Controllers;

[Culture]
public class HomeController : Controller
{
    readonly ILogger<HomeController> logger;
    readonly ApplicationDbContext context;
    readonly IWebHostEnvironment webHostEnv;
    readonly UserManager<ApplicationUser> userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnv, UserManager<ApplicationUser> userManager)
    {
        this.logger = logger;
        this.context = context;
        this.webHostEnv = webHostEnv;
        this.userManager = userManager;
    }

    public async Task<ActionResult> Index(int? page)
    {
        int pageNumber = page ?? 1;
        int pageSize = 3;

        if (pageNumber < 1)
            NotFound();

        var posts = context.Posts
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, pageSize);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.TotalCount = await context.Posts.CountAsync();
        ViewBag.Page = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / pageSize);
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.TotalPages)
            NotFound();

        return View(posts);
    }

    [Route("/About")]
    public IActionResult About()
    {
        return RedirectToAction(nameof(Index));
    }

    [Route("/Post")]
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

        if (postsWithTag == null || !postsWithTag.Any()) return RedirectToAction(nameof(Index));

        return View();
    }

    [Route("/News")]
    public async Task<ActionResult> News()
    {
        var news = await context.Posts
        .Where(x => x.TypeId == Models.PostType.Новина)
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

    [Route("/Reviews")]
    public async Task<ActionResult> Reviews()
    {
        var reviews = await context.Posts
            .Where(x => x.TypeId == Models.PostType.Огляд)
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

    [Route("/Articles")]
    public async Task<ActionResult> Articles()
    {
        var reviews = await context.Posts
            .Where(x => x.TypeId == Models.PostType.Стаття)
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

    [Route("/Guides")]
    public async Task<ActionResult> Guides()
    {
        var reviews = await context.Posts
            .Where(x => x.TypeId == Models.PostType.Гайд)
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
        var coments = await context.Comments
            .Where(x => x.PostId == id)
            .ToListAsync();

        ViewBag.Post = post;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Show(int id, string author, string text, int? replyId)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(text))
            {
                Comment coment = new(id, author, text, false, replyId);
                await context.Comments.AddAsync(coment);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Show), id);
        }

        return NotFound();
    }

    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        return View();
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create(Post post, IFormFile file)
    {
        // if (string.IsNullOrEmpty(post.ImageUrl))
        //     ModelState.AddModelError(nameof(post.ImageUrl), "Де файл?!");

        if (ModelState.IsValid)
        {
            if (file != null && file.Length > 0)
            {
                // Збережіть файл у папці wwwroot або в потрібному вам шляху
                var uploadsDirectory = Path.Combine(webHostEnv.WebRootPath, "post_title_image");
                if (!Directory.Exists(uploadsDirectory)) Directory.CreateDirectory(uploadsDirectory);

                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(webHostEnv.WebRootPath, "post_title_image", uniqueFileName);

                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                post.TitleImage = "/post_title_image/" + uniqueFileName;
            }

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        return View(post);
    }

    [Authorize]
    public async Task<ActionResult> Edit(int id)
    {
        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
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
                string uploadsDirectory = Path.Combine(webHostEnv.WebRootPath, "post_title_image");
                if (!Directory.Exists(uploadsDirectory)) Directory.CreateDirectory(uploadsDirectory);

                string fileExtension = Path.GetExtension(file.FileName);
                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(webHostEnv.WebRootPath, "post_title_image", uniqueFileName);

                using (FileStream fileStream = new(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                post.TitleImage = "/post_title_image/" + uniqueFileName;
            }
            else
            {
                // Якщо файл не був вибраний, використовуйте поточне зображення
                var existingPost = await context.Posts.FindAsync(post.Id);
                post.TitleImage = existingPost?.TitleImage;
            }

            context.Posts.Update(post);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        post.TitleImage = context.Posts.Find(post.Id)?.TitleImage;
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
