using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GameSite.Controllers;

[Route("AdminPanel"), Authorize(Roles = "Admin")]
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

    [Route("/AdminPanel")]
    public async Task<IActionResult> AdminPanel()
    {
        ViewBag.Roles = await context.Roles.ToListAsync();
        ViewBag.Users = await context.ApplicationUsers.ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<ActionResult> ManageRole(string submitType, string userId, string role)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(userId);
        if (user == null || submitType.IsNullOrEmpty())
        {
            return RedirectToAction("Index", "Home");
        }

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

        return RedirectToAction(nameof(AdminPanel));
    }
}


