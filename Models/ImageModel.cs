using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameSite.Models;

public class ImageModel
{
    public int Id { get; set; }
    public string ImageTitle { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}