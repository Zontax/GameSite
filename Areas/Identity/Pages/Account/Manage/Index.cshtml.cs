﻿#nullable disable
using System.ComponentModel.DataAnnotations;
using GameSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameSite.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    readonly UserManager<ApplicationUser> _userManager;
    readonly SignInManager<ApplicationUser> _signInManager;

    public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [Display(Name = "UserName", ResourceType = typeof(Resources.Resource))]
    public string Username { get; set; }

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
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Required(
            ErrorMessageResourceType = typeof(Resources.Resource),
            ErrorMessageResourceName = "EnterTheNameThatWillBeDisplayedOnTheSite")]
        [MaxLength(25)]
        [Display(Name = "_Name", ResourceType = typeof(Resources.Resource))]
        public string Name { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Phone]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Resource))]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Гендер")]
        public bool? Gender { get; set; }

        [MaxLength(200)]
        [Display(Name = "Опис")]
        public string Description { get; set; }
    }

    async Task LoadAsync(ApplicationUser user)
    {
        var name = user.Name;
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        var description = user.Description;
        var gender = user.Gender;

        Username = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber,
            Name = name,
            Description = description,
            Gender = gender,
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        if (Input.Name != user.Name)
        {
            user.Name = Input.Name.Trim();
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = Resources.Resource.Error;
                return RedirectToPage();
            }
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = Resources.Resource.Error;
                return RedirectToPage();
            }
        }

        if (Input.Description != user.Description)
        {
            user.Description = Input.Description;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = Resources.Resource.Error;
                return RedirectToPage();
            }
        }

        if (Input.Gender != user.Gender)
        {
            user.Gender = Input.Gender;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                StatusMessage = Resources.Resource.Error;
                return RedirectToPage();
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = Resources.Resource.YourProfileHasBeenUpdated;

        return RedirectToAction("Profile", new { id = user.Id });
    }
}
