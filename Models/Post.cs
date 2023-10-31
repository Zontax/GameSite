using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Post
{
    public int Id { get; set; }

    [Display(Name = "Тип публікації")]
    public Type TypeId { get; set; } = Type.Новина;

    public string UrlSlug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [StringLength(150, ErrorMessage = "не більше 100 символів")]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [Display(Name = "Текст")]
    public string Content { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [Display(Name = "Автор")]
    public string Author { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.Now;

    public int LikesCount { get; set; } = 0;

    public int DislikesCount { get; set; } = 0;

    [Required(ErrorMessage = "Введіть теги")]
    [Display(Name = "Теги")]
    public string Tags { get; set; } = string.Empty;

    [Display(Name = "Гра")]
    public int? GameId { get; set; } = 0;

    public Post()
    {
        UrlSlug = Id.ToString();
    }

    public Post(Type type, string title, string content, string author)
    {
        UrlSlug = Id.ToString();
        TypeId = type;
        Title = title;
        Content = content;
        Author = author;
    }

    public Post(Type type, string title, string content, string author, DateTime date)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Date = date;
    }

    public Post(Type type, string title, string content, string author, string tags, int likes = 0, int dislikes = 0, int gameId = 0)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Tags = tags;
    }

    public string GetColorCode(Type typeId)
    {
        switch (typeId)
        {
            case Type.Новина:
                return "blue";
            case Type.Огляд:
                return "green";
            case Type.Стаття:
                return "red";
            case Type.Гайд:
                return "orange";
            case Type.Відео:
                return "purple";
            case Type.Подкаст:
                return "brown";
            default:
                return "black";
        }
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

