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

namespace GameSite.Controllers;

[Culture]
public class AjaxController : Controller
{
    readonly ILogger<AjaxController> logger;
    readonly ApplicationDbContext context;
    readonly IWebHostEnvironment webHostEnv;
    readonly UserManager<ApplicationUser> userManager;

    public AjaxController(ILogger<AjaxController> logger, ApplicationDbContext context, IWebHostEnvironment webHostEnv, UserManager<ApplicationUser> userManager)
    {
        this.logger = logger;
        this.context = context;
        this.webHostEnv = webHostEnv;
        this.userManager = userManager;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Like(int postId, string userId)
    {
        Post? post = await context.Posts
            .Include(p => p.LikedByUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);
        ApplicationUser? user = await context.ApplicationUsers.FindAsync(userId);

        if (post != null && user != null)
        {
            if (post.LikedByUsers != null && post.LikedByUsers.Contains(user))
            {
                post.LikedByUsers.Remove(user);
                post.LikesCount--;
                await context.SaveChangesAsync();
                return Json(new { success = false });
            }
            else
            {
                if (post.LikedByUsers == null)
                    post.LikedByUsers = new List<ApplicationUser>();

                post.LikedByUsers.Add(user);
                post.LikesCount++;
                await context.SaveChangesAsync();
                return Json(new { success = true });
            }
        }

        return Json(new { success = false });
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Dislike(int postId)
    {
        await context.SaveChangesAsync();

        return Json(new { success = true });
    }
}