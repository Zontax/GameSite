using Microsoft.AspNetCore.Identity;
using GameSite.Models;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Data;

// Summary:
//     Користувач сайта на основі IdentityUser
[Index(nameof(Name))]
public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? AvatarImage { get; set; }
    public bool? Gender { get; set; }
    public int? Age { get; set; }
    public virtual ICollection<Post>? LikedPosts { get; set; }
    public virtual ICollection<Post>? DislikedPosts { get; set; }
    public virtual ICollection<Post>? SavedPosts { get; set; }
}
