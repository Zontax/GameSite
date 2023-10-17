using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Publication
{
    public int Id { get; set; }
    [Display(Name = "Тип публікації")]
    public Type TypeId { get; set; } = Type.Новина;
    public string UrlSlug { get; set; } = string.Empty;
    [Required]
    [StringLength(100)]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;
    [Display(Name = "Текст")]
    [Required]
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    [Required]
    [Display(Name = "Автор")]
    public string Author { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public int LikesCount { get; set; } = 0;
    public int DislikesCount { get; set; } = 0;
    [Display(Name = "Теги")]
    public string Tags { get; set; } = string.Empty;
    [Display(Name = "Гра")]
    public int GameId { get; set; } = 0;

    public Publication()
    {
        UrlSlug = Id.ToString();
    }

    public Publication(Type type, string title, string content, string author)
    {
        UrlSlug = Id.ToString();
        TypeId = type;
        Title = title;
        Content = content;
        Author = author;
    }

    public Publication(Type type, string title, string content, string author, DateTime date)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Date = date;
    }

    public Publication(Type type, string title, string content, string author, int likes = 0, int dislikes = 0, string tags = "", int gameId = 0)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
    }
}

public enum Type
{
    Новина,
    Огляд,
    Стаття,
    Гайд,
    Відео,
    Подкаст,
}
