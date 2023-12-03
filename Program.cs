using GameSite.Data;
using GameSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SQLiteConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString)); //  SQLite
                                          //options.UseSqlServer(connectionString)); // MSSQL Server

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapDefaultControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapControllerRoute(
//         name: "home",
//         pattern: "{action}",
//         defaults: new { controller = "Home", action = "Index" });

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    PostDbInitializer.Initialize(context);
}
app.Run();

// void CreateRoles(IServiceProvider serviceProvider)
// {
//     var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//     var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//     Task<IdentityResult> roleResult;
//     string email = "jrvadim19@gmail.com";

//     //Check that there is an Administrator role and create if not
//     Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Administrator");
//     hasAdminRole.Wait();

//     if (!hasAdminRole.Result)
//     {
//         roleResult = roleManager.CreateAsync(new IdentityRole("Administrator"));
//         roleResult.Wait();
//     }

//     //Check if the admin user exists and create it if not
//     //Add to the Administrator role

//     Task<ApplicationUser> testUser = userManager.FindByEmailAsync(email);
//     testUser?.Wait();

//     if (testUser.Result == null)
//     {
//         ApplicationUser administrator = new()
//         {
//             Email = email,
//             UserName = email.ToUpper(),
//             EmailConfirmed = true,
//         };

//         Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "asdASD12345#");
//         newUser.Wait();

//         if (newUser.Result.Succeeded)
//         {
//             Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Administrator");
//             newUserRole.Wait();
//         }
//     }
// }
