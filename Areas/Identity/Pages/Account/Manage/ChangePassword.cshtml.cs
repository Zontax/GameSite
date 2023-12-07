#nullable disable
using System.ComponentModel.DataAnnotations;
using GameSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSite.Areas.Identity.Pages.Account.Manage;

public class ChangePasswordModel : PageModel
{
    readonly UserManager<ApplicationUser> _userManager;
    readonly SignInManager<ApplicationUser> _signInManager;
    readonly ILogger<ChangePasswordModel> _logger;

    public ChangePasswordModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<ChangePasswordModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Поточний пароль")]
        public string OldPassword { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0} має містити принаймні {2} і не більше {1} символів.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Підтвердити новий пароль")]
        [Compare(nameof(NewPassword), ErrorMessage = "Паролі не збігаються.")]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Не вдалося завантажити користувача з ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword)
        {
            return RedirectToPage("./SetPassword");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        _logger.LogInformation("User changed their password successfully.");
        StatusMessage = "Your password has been changed.";

        return RedirectToPage();
    }
}
