#nullable disable
using GameSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSite.Areas.Identity.Pages.Account.Manage;

public class TwoFactorAuthenticationModel : PageModel
{
    readonly UserManager<ApplicationUser> _userManager;
    readonly SignInManager<ApplicationUser> _signInManager;
    readonly ILogger<TwoFactorAuthenticationModel> _logger;

    public TwoFactorAuthenticationModel(
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<TwoFactorAuthenticationModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool HasAuthenticator { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public int RecoveryCodesLeft { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public bool Is2faEnabled { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool IsMachineRemembered { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Не вдалося завантажити користувача з ID '{_userManager.GetUserId(User)}'.");
        }

        HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
        Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
        RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Не вдалося завантажити користувача з ID '{_userManager.GetUserId(User)}'.");
        }

        await _signInManager.ForgetTwoFactorClientAsync();
        StatusMessage = "Поточний браузер забуто. Коли ви знову ввійдете з цього браузера, вам буде запропоновано ввести код 2fa.";
        return RedirectToPage();
    }
}
