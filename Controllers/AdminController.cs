using GameSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GameSite.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    readonly ApplicationDbContext context;
    readonly UserManager<ApplicationUser> userManager;
    readonly RoleManager<IdentityRole> roleManager;
    
    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.context = context;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    public async Task<IActionResult> Users(int? page)
    {
        if (page == null || page < 1) page = 1;
        if (page < 1) return NotFound();

        var users = await context.ApplicationUsers.ToListAsync();

        Pager pager = new(users.Count(), page, 10, "Users", "Admin");
        users = users.ToPagedList(pager);

        ViewBag.Pager = pager;
        ViewBag.Users = users;
        ViewBag.Roles = await context.Roles.ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> ManageRole(string submitType, string userId, string role)
    {
		var currentUser = await userManager.GetUserAsync(User);
		ApplicationUser? user = await userManager.FindByIdAsync(userId);

        if (user == null || submitType.IsNullOrEmpty())
            return RedirectToAction("Index", "Home");

        switch (submitType)
        {
            case "Add":
                if (!role.IsNullOrEmpty())
                {
                    role = role.Trim();
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await userManager.AddToRoleAsync(user, role);
                    await context.SaveChangesAsync();
                }
                break;

            case "Remove":
                if (!role.IsNullOrEmpty())
                {
                    role = role.Trim();

                    if (role.ToLower() == "admin" && currentUser == user)
						break;

					await userManager.RemoveFromRoleAsync(user, role);
                    await context.SaveChangesAsync();
                }
                break;

            case "Block":
                user.LockoutEnd = DateTimeOffset.MaxValue;
                await userManager.UpdateAsync(user);
                break;

            case "Unblock":
                user.LockoutEnd = null;
                await userManager.UpdateAsync(user);
                break;
        }

        return RedirectToAction(nameof(Users));
    }

    public async Task<IActionResult> DeleteComment(int commentId, int postId)
    {
        var comment = await context.Comments.FindAsync(commentId);

        if (comment == null) return NotFound();

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();

        return Redirect(Url.Action("Show", "Home", new { id = postId }) + "#comments");
    }
}


