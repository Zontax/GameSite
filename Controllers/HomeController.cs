using System.Diagnostics;
using System.Text.RegularExpressions;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using X.PagedList;

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

    public async Task<IActionResult> Index(int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        var posts = context.Posts
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, 4);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.CountAsync(), pageNumber, 4, nameof(Index));
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View(posts);
    }

    public async Task<IActionResult> News(int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        var posts = context.Posts
            .Where(x => x.TypeId == Models.PostType.Новина)
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, 4);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.Where(p => p.TypeId == PostType.Новина).CountAsync(), pageNumber, 4, nameof(News));
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View(posts);
    }

    public async Task<IActionResult> Reviews(int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        var posts = context.Posts
            .Where(x => x.TypeId == Models.PostType.Огляд)
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, 4);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.Where(p => p.TypeId == PostType.Новина).CountAsync(), pageNumber, 4, nameof(Reviews));
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View(posts);
    }

    public async Task<IActionResult> Articles(int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        var posts = context.Posts
            .Where(x => x.TypeId == Models.PostType.Стаття)
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, 4);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.Where(p => p.TypeId == PostType.Стаття).CountAsync(), pageNumber, 4, nameof(Articles));
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> Guides(int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        var posts = context.Posts
            .Where(x => x.TypeId == Models.PostType.Гайд)
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, 4);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.Where(p => p.TypeId == PostType.Гайд).CountAsync(), pageNumber, 4, nameof(Guides));
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View(posts);
    }

    [Authorize]
    public async Task<IActionResult> Saved(int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        ApplicationUser? user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        var posts = context.Posts
            .Where(post => post.SavedByUsers.Any(u => u.Id == user.Id))
            .OrderByDescending(post => post.Date)
            .ToPagedList(pageNumber, 4);

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.Where(post => post.SavedByUsers.Any(u => u.Id == user.Id)).CountAsync(), pageNumber, 4, nameof(Saved));
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        return View();
    }

    [Route("/Post")]
    public async Task<IActionResult> Tag(string tag, int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        string[] tags = tag.Split(new[] { ',' });
        var searchTags = string.Join("|", tags.Select(t => $"\\b{Regex.Escape(t.Trim())}\\b"));

        var posts = context.Posts
            .Where(post => Regex.IsMatch(post.Tags, searchTags))
            .ToPagedList(pageNumber, 4);

        if (!posts.Any()) return NotFound();

        var commentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.PG = new Pagination(await context.Posts.Where(post => Regex.IsMatch(post.Tags, searchTags)).CountAsync(), pageNumber, 4, nameof(Tag), tag);
        ViewBag.Posts = posts;
        ViewBag.CommentsCount = commentsCount;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> Search(string? search, int? page)
    {
        int pageNumber = page ?? 1;
        if (pageNumber < 1) return NotFound();

        search = search?.Trim();
        if (string.IsNullOrEmpty(search) || search.Length < 3)
            search = string.Empty;

        var posts = await context.Posts
           .Where(s => s.Title!.ToLower().Contains(search.ToLower()) ||
                   s.Content!.ToLower().Contains(search.ToLower()) ||
                   s.Tags.ToLower().Contains(search.ToLower()))
           .ToPagedListAsync(pageNumber, 4);

        ViewBag.CommentsCount = await context.Comments
            .GroupBy(com => com.PostId)
            .Select(group => new { PostId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(i => i.PostId, i => i.Count);

        ViewBag.Search = search;

        if (!posts.Any() || string.IsNullOrEmpty(search))
        {

            return View();
        }

        ViewBag.PG = new Pagination(posts.Count, pageNumber, 4, nameof(Search), searchParametr: search);
        ViewBag.Posts = posts;

        if (pageNumber > ViewBag.PG.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> Show(int id)
    {
        var post = await context.Posts
            .FindAsync(id);

        if (post == null) return NotFound();

        var coments = await context.Comments
            .Where(x => x.PostId == id)
            .ToListAsync();

        ViewBag.Post = post;
        ViewBag.Coments = coments;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Show(int id, string author, string text, int? replyId)
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

    [Authorize(Roles = "Author")]
    public ActionResult Create()
    {
        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        return View();
    }

    [HttpPost, Authorize(Roles = "Author")]
    public async Task<ActionResult> Create(Post post, IFormFile file)
    {
        if (file == null)
            ModelState.AddModelError(nameof(post.TitleImage), Resources.Resource.TitleFileRequired);

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

    [Authorize(Roles = "Author")]
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        var post = await context.Posts.FindAsync(id);

        if (post == null) return NotFound();
        return View(await context.Posts.FindAsync(id));
    }

    [HttpPost, Authorize(Roles = "Author")]
    public async Task<IActionResult> Edit(Post post, IFormFile file)
    {
        if (file == null)
            ModelState.AddModelError(nameof(post.TitleImage), Resources.Resource.TitleFileRequired);

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

        if (ModelState.IsValid)
        {
            context.Posts.Update(post);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Show), new { id = post.Id });
        }

        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        post.TitleImage = context.Posts.Find(post.Id)?.TitleImage;
        await context.SaveChangesAsync();
        return View(post);
    }

    [Authorize(Roles = "Author")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await context.Posts.FindAsync(id);

        if (post == null) return NotFound();

        context.Posts.Remove(post);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Route("/About")]
    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
