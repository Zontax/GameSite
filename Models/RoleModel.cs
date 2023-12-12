using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class RoleModel
{
    [Required]
    public string RoleName { get; set; } = null!;
}
