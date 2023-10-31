using Microsoft.AspNetCore.Identity;

namespace GameSite.Data;

public class ApplicationIdentityUser : IdentityUser
{
    public string CustomRole { get; set; }
}