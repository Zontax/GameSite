using Microsoft.AspNetCore.Identity;

namespace GameSite.Data;

// Summary:
//     Користувач сайта на основі IdentityUser
public class ApplicationUser : IdentityUser
{
    public DateTime RegistrationDate { get; set; }
    public int? Years { get; set; }
    // Своє поле для ролі користувача
    public string? Role { get; set; }
}

