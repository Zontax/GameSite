using Microsoft.AspNetCore.Identity;
using GameSite.Models;

namespace GameSite.Data;

// Summary:
//     Користувач сайта на основі IdentityUser
public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int Lang { get; set; } = 0;
    public DateTime RegistrationDate { get; set; }
    public string? AvatarImage { get; set; }
    public bool? Gender { get; set; }
    public int? Age { get; set; }
    public string? Role { get; set; }
    public virtual ICollection<Post>? LikedPosts { get; set; }
    public virtual ICollection<Post>? DislikedPosts { get; set; }
    public virtual ICollection<Post>? SavedPosts { get; set; }
}

public enum UserLang
{
    Ukraine,
    English
}
