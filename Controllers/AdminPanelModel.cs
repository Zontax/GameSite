// using System.Data;
// using GameSite.Data;
// using GameSite.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc.RazorPages;

// namespace GameSite.Pages;

// public class AdminPanelModel : PageModel
// {
//     readonly ILogger<AdminPanelModel> logger;
//     readonly RoleManager<IdentityRole> roleManager;
//     readonly UserManager<ApplicationUser> userManager;
//     readonly ApplicationDbContext context;

//     public AdminPanelModel(ILogger<AdminPanelModel> logger,
//         RoleManager<IdentityRole> roleManager,
//         UserManager<ApplicationUser> userManager,
//         ApplicationDbContext context)
//     {
//         this.logger = logger;
//         this.context = context;
//         this.roleManager = roleManager;
//         this.userManager = userManager;
//     }

//     public async Task OnPostNewRole(RoleModel model)
//     {
//         string roleName = model.RoleName.Trim();

//         if (!string.IsNullOrEmpty(roleName))
//         {
//             if (!await roleManager.RoleExistsAsync(roleName))
//             {
//                 // Create new role
//                 await roleManager.CreateAsync(new IdentityRole
//                 {
//                     Name = roleName,
//                     NormalizedName = roleName
//                 });
//             }
//         }
//     }

//     public async Task OnPostAddRole(RoleModel model)
//     {
//         string roleName = model.RoleName.Trim();

//         if (!string.IsNullOrEmpty(roleName))
//         {
//             var user = await userManager.GetUserAsync(User);
//             await userManager.AddToRoleAsync(user, roleName);
//         }

//     }

//     public async Task OnPostRemoveRole(RoleModel model)
//     {
//         string roleName = model.RoleName.Trim();

//         if (!string.IsNullOrEmpty(roleName))
//         {
//             var user = await userManager.GetUserAsync(User);
//             await userManager.RemoveFromRoleAsync(user, roleName);
//         }
//     }
// }
