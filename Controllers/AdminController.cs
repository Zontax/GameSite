using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IActionResult> Index()
    {
        var users = await context.ApplicationUsers.ToListAsync();
        ViewBag.Roles = await context.Roles.ToListAsync();

        return View(users);
    }

    [HttpPost]
    public async Task<ActionResult> ManageRole(string submitType, string userId, string role)
    {
        role = role.Trim();
        ApplicationUser? user = await userManager.FindByIdAsync(userId);

        if (submitType == "Add")
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            if (user != null)
                await userManager.AddToRoleAsync(user, role);

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else if (submitType == "Remove")
        {
            if (user != null)
                await userManager.RemoveFromRoleAsync(user, role);

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }
}
