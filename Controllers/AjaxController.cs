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
using Resources;

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
    public async Task<IActionResult> LikeOrDislike(int postId, string userId, string action)
    {
        ApplicationUser? user = await context.ApplicationUsers.FindAsync(userId);
        Post? post = await context.Posts
            .Include(p => p.LikedByUsers)
            .Include(p => p.DislikedByUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post != null && user != null)
        {
            if (action == "like")
            {
                if (post.LikedByUsers != null && post.LikedByUsers.Contains(user))
                {
                    post.LikedByUsers.Remove(user);
                    if (post.LikesCount != 0)
                        post.LikesCount--;
                }
                else
                {
                    if (post.LikedByUsers == null)
                        post.LikedByUsers = new List<ApplicationUser>();

                    post.LikedByUsers.Add(user);
                    post.LikesCount++;
                }

                if (post.DislikedByUsers != null && post.DislikedByUsers.Contains(user))
                {
                    post.DislikedByUsers.Remove(user);
                    if (post.DislikesCount != 0)
                        post.DislikesCount--;
                }
            }
            else if (action == "dislike")
            {
                if (post.DislikedByUsers != null && post.DislikedByUsers.Contains(user))
                {
                    post.DislikedByUsers.Remove(user);
                    if (post.DislikesCount != 0)
                        post.DislikesCount--;
                }
                else
                {
                    if (post.DislikedByUsers == null)
                        post.DislikedByUsers = new List<ApplicationUser>();

                    post.DislikedByUsers.Add(user);
                    post.DislikesCount++;
                }

                if (post.LikedByUsers != null && post.LikedByUsers.Contains(user))
                {
                    post.LikedByUsers.Remove(user);
                    if (post.LikesCount != 0)
                        post.LikesCount--;
                }
            }

            await context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                likes = post.LikesCount,
                dislikes = post.DislikesCount
            });
        }

        return Json(new { success = false, likes = post?.LikesCount, dislikes = post?.DislikesCount });
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> AddToSaved(int postId)
    {
        var user = await userManager.GetUserAsync(User);
        var post = await context.Posts
            .Include(p => p.SavedByUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post != null && user != null)
        {
            if (post.SavedByUsers != null && post.SavedByUsers.Contains(user))
            {
                post.SavedByUsers.Remove(user);
                await context.SaveChangesAsync();
                return Json(new { success = false, message = Resources.Resource.Save });
            }
            else
            {
                if (post.SavedByUsers == null)
                    post.SavedByUsers = new List<ApplicationUser>();

                post.SavedByUsers.Add(user);
                await context.SaveChangesAsync();
                return Json(new { success = false, message = Resources.Resource.InSaved });
            }
        }

        return Json(new { success = false, message = Resources.Resource.Error });
    }

    [Authorize]
    public async Task<IActionResult> CheckSaved(int postId)
    {
        var user = userManager.GetUserAsync(User).Result;
        var post = await context.Posts
            .Include(p => p.SavedByUsers)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post != null && user != null)
        {
            bool isSaved = post.SavedByUsers != null && post.SavedByUsers.Contains(user);
            if (isSaved)
                return Json(new { message = Resources.Resource.InSaved });
        }

        return Json(new { message = Resources.Resource.Save });
    }
}
