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

    [Display(Name = "Обкладинка публікації")]
    public string? ImageUrl { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [Display(Name = "Автор")]
    public string Author { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int LikesCount { get; set; }

    public int DislikesCount { get; set; }

    [Required(ErrorMessage = "Введіть теги")]
    [Display(Name = """Додайте теги таким чином "тег1, тег2, мій_третій_тег" через кому """)]
    public string Tags { get; set; } = string.Empty;

    [Display(Name = "Чи редаговано")]
    public bool Edited { get; set; } = false;

    [Display(Name = "Дата редагування")]
    public DateTime EditedDate { get; set; } = DateTime.UtcNow;

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

    public Post(Type type, string title, string content, string author, string tags, int likes = 0, int dislikes = 0)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Tags = tags;
        LikesCount = likes;
        DislikesCount = dislikes;
    }

    public Post(Type type, string title, string content, string author, string tags, DateTime date, int likes = 0, int dislikes = 0, string imageUrl = "")
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Tags = tags;
        LikesCount = likes;
        DislikesCount = dislikes;
        Date = date;
        ImageUrl = imageUrl;
    }

    public string GetColorCode(Type typeId)
    {
#pragma warning disable IDE0066
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
#pragma warning restore IDE0066
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

