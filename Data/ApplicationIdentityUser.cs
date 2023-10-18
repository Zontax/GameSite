using Microsoft.AspNetCore.Identity;

namespace GameSite.Data;

public class ApplicationIdentityUser : IdentityUser
{
    public long AppId { get; set; }
}