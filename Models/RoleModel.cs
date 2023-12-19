using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class RoleModel
{
    [Required(ErrorMessage = "Поле 'Назва ролі' є обов'язковим")]
    public string RoleName { get; set; }
}
