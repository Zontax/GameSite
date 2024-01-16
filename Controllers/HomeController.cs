using System.Diagnostics;
using System.Text.RegularExpressions;
using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Controllers;

[Culture]
public class HomeController : Controller
{
    readonly ApplicationDbContext context;
    readonly IWebHostEnvironment webHostEnv;
    readonly UserManager<ApplicationUser> userManager;

    public HomeController(ApplicationDbContext context, IWebHostEnvironment webHostEnv, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.webHostEnv = webHostEnv;
        this.userManager = userManager;
    }

    async Task<IActionResult> GetPostsByType(PostType type, int? page, string returnPage)
    {
        if (page == null || page < 1) page = 1;
        if (page < 1) return NotFound(); ;

        var posts = await context.Posts
            .AsNoTracking()
            .Include(p => p.Comments)
            .Where(x => x.TypeId == type)
            .OrderByDescending(post => post.Date)
            .ToListAsync();

        Pager pager = new(posts.Count(), page, 7, returnPage);
        posts = posts.ToPagedList(pager);

        ViewBag.Pager = pager;
        ViewBag.Posts = posts;

        if (page > pager.TotalPages) return NotFound();

        return View(posts);
    }

    public async Task<IActionResult> Index(int? page)
    {
        if (page == null || page < 1) page = 1;
        if (page < 1) return NotFound();

        var posts = await context.Posts
            .AsNoTracking()
            .Include(p => p.Comments)
            .OrderByDescending(post => post.Date)
            .ToListAsync();

        Pager pager = new(posts.Count(), page, 7, "Index");
        posts = posts.ToPagedList(pager);


        ViewBag.Pager = pager;
        ViewBag.Posts = posts;

        if (page > pager.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> News(int? page)
    {
        return await GetPostsByType(PostType.Новина, page, "News");
    }

    public async Task<IActionResult> Reviews(int? page)
    {
        return await GetPostsByType(PostType.Огляд, page, "Reviews");
    }

    public async Task<IActionResult> Articles(int? page)
    {
        return await GetPostsByType(PostType.Стаття, page, "Articles");
    }

    public async Task<IActionResult> Guides(int? page)
    {
        return await GetPostsByType(PostType.Гайд, page, "Guides");
    }

    [Authorize]
    public async Task<IActionResult> Saved(int? page)
    {
        if (page == null || page < 1) page = 1;
        if (page < 1) return NotFound();

        ApplicationUser? user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

#pragma warning disable CS8604

        var posts = await context.Posts
            .Include(p => p.Comments)
            .Where(post => post.SavedByUsers.Any(u => u.Id == user.Id))
            .OrderByDescending(post => post.Date)
            .ToListAsync();

#pragma warning restore CS8604

        Pager pager = new(posts.Count(), page, 7, "Saved");
        posts = posts.ToPagedList(pager);

        ViewBag.Pager = pager;
        ViewBag.Posts = posts;

        if (page > pager.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> Tag(string tag, int? page)
    {
        if (string.IsNullOrEmpty(tag)) return NotFound();
        if (page == null || page < 1) page = 1;

        string[] tags = tag.Split(new[] { ',' });

        var postsQuery = context.Posts
            .Include(p => p.Comments)
            .AsQueryable();

        foreach (var t in tags)
        {
            postsQuery = postsQuery.Where(post => post.Tags.Contains(t.Trim()));
        }

        var posts = await postsQuery.ToListAsync();

        if (!posts.Any()) return NotFound();

        Pager pager = new(posts.Count(), page, 7, "Tag", "Home", tag);
        posts = posts.ToPagedList(pager);

        ViewBag.Tag = tag.Replace('_', ' ');
        ViewBag.Pager = pager;
        ViewBag.Posts = posts;

        if (page > pager.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> Search(string? search, int? page)
    {
        if (page == null || page < 1) page = 1;

        search = search?.Trim().ToLower();
        if (string.IsNullOrEmpty(search) || search.Length < 3)
        {
            ViewBag.Search = search;
            return View();
        }

        var posts = await context.Posts
            .Include(p => p.Comments)
            .Where(s => s.Title!.ToLower().Contains(search) ||
                        s.Content!.ToLower().Contains(search) ||
                        s.Tags.ToLower().Contains(search)).ToListAsync();

        if (!posts.Any()) // Коли нічого не знайдено
        {
            ViewBag.Search = search;
            return View();
        }

        Pager pager = new(posts.Count(), page, 7, "Search", "Home", null, search);
        posts = posts.ToPagedList(pager);

        ViewBag.Search = search;
        ViewBag.Pager = pager;
        ViewBag.Posts = posts;

        if (page > pager.TotalPages) return NotFound();

        return View();
    }

    public async Task<IActionResult> Show(int id)
    {
        var post = await context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null) return NotFound();

        ViewBag.Post = post;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Show(int postId, string author, string text, int? replyId)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(author) && !string.IsNullOrEmpty(text))
            {
                Comment coment = new(postId, author, text, false, replyId);
                await context.Comments.AddAsync(coment);
                await context.SaveChangesAsync();
            }

            return Redirect(Url.Action(nameof(Show), new { postId = postId }) + "#comments");
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
                post.TitleImage = await SaveFileAsync(file, "post_title_image");
            }

            await context.Posts.AddAsync(post);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Show), new { id = post.Id });
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
    public async Task<IActionResult> Edit(Post post, IFormFile? file)
    {
        if (file != null && file.Length > 0)
        {
            // Видалення попереднього зображення
            string? oldTitleImage = await context.Posts
                .AsNoTracking()
                .Where(p => p.Id == post.Id)
                .Select(p => p.TitleImage)
                .FirstOrDefaultAsync();

            DeleteFile(oldTitleImage); // Видалення попереднього зображення

            post.TitleImage = await SaveFileAsync(file, "post_title_image");

        }
        else
        {
            // Якщо файл не був вибраний, використовуйте поточне зображення
            post.TitleImage = await context.Posts
                .AsNoTracking()
                .Where(p => p.Id == post.Id)
                .Select(p => p.TitleImage)
                .FirstOrDefaultAsync();
        }

        if (ModelState.IsValid)
        {
            DateTime oldDateTime = await context.Posts
                .AsNoTracking()
                .Where(p => p.Id == post.Id)
                .Select(p => p.Date)
                .FirstOrDefaultAsync();

            post.Date = oldDateTime;

            context.Posts.Update(post);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Show), new { id = post.Id });
        }

        ViewBag.SelectItems = new SelectList(Enum.GetValues(typeof(Models.PostType)));
        return View(post);
    }

    [Authorize(Roles = "Author")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await context.Posts.FindAsync(id);

        if (post == null) return NotFound();

        DeleteFile(post.TitleImage);
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

    private async Task<string> SaveFileAsync(IFormFile file, string directoryPath)
    {
        string uploadsDirectory = Path.Combine(webHostEnv.WebRootPath, directoryPath);
        if (!Directory.Exists(uploadsDirectory)) Directory.CreateDirectory(uploadsDirectory);

        string fileExtension = Path.GetExtension(file.FileName);
        string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        string filePath = Path.Combine(uploadsDirectory, uniqueFileName);

        using (FileStream fileStream = new(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return "/" + directoryPath + "/" + uniqueFileName;
    }

    private void DeleteFile(string? filePath)
    {
        if (filePath != null)
        {
            var deletePath = Path.Combine(webHostEnv.WebRootPath, filePath[1..]);
            if (System.IO.File.Exists(deletePath))
                System.IO.File.Delete(deletePath);
        }
    }
}
