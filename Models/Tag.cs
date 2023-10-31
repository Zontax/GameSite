using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UrlSlug { get; set; }
    public string Description { get; set; }
    public List<Post> Posts { get; set; } = new();

    public Tag(string name, string description = "")
    {
        Name = name;
        UrlSlug = name;
        Description = description;
    }
}
