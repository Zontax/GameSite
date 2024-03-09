using GameSite.Data;
using Microsoft.AspNetCore.Identity;

namespace GameSite.Models;

public class AdminInitializer
{
    public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
		/* if (context.Posts.Any())
        {
            // context.Posts.ExecuteDelete();
            // context.Comments.ExecuteDelete();
            // context.SaveChanges();
            return;
        } */
	}
}

