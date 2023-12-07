using GameSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Controllers;

public class AdminController : Controller
{
    readonly ApplicationDbContext context;
    readonly UserManager<ApplicationUser> userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var users = await context.ApplicationUsers.ToListAsync();

        return View(users);
    }
}