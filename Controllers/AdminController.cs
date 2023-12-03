using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameSite.Controllers;

public class AdminController : Controller
{
    readonly ApplicationDbContext context;
    readonly UserManager<IdentityUser> userManager;

    public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        this.context = context;
        this.userManager = userManager;
    }

    // public IActionResult Index()
    // {
    //     //var users = userManager.Users.ToList();
    //     var users = context.ApplicationUsers.ToList();
    //     return View(users);
    // }
}