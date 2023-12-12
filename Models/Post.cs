using System.ComponentModel.DataAnnotations;
using GameSite.Data;

namespace GameSite.Models;

public class Post
{
    public int Id { get; set; }

    [Display(Name = "Тип публікації")]
    public PostType TypeId { get; set; } = PostType.Новина;

    public string UrlSlug { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [StringLength(140, ErrorMessage = "не більше 140 символів")]
    [Display(Name = "Назва")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [Display(Name = "Текст")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Обкладинка публікації")]
    public string? TitleImage { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле обов'язкове")]
    [Display(Name = "Автор")]
    public string Author { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int LikesCount { get; set; }

    public virtual ICollection<ApplicationUser>? LikedByUsers { get; set; }

    public int DislikesCount { get; set; }

    [Required(ErrorMessage = "Введіть теги")]
    [Display(Name = """Додайте теги таким чином "тег1, тег2, мій_третій_тег" через кому """)]
    public string Tags { get; set; } = string.Empty;

    [Display(Name = "Чи редаговано")]
    public bool Edited { get; set; } = false;

    [Display(Name = "Дата редагування")]
    public DateTime EditedDate { get; set; } = DateTime.UtcNow;

    //// Якщо тип Огляд
    [Display(Name = "Гра на огляді")]
    public string? ReviewGameId { get; set; } = string.Empty;

    [Display(Name = "Оцінка")]
    public string? ReviewRating { get; set; } = string.Empty;

    [Display(Name = "Плюси")]
    public string? ReviewPlus { get; set; } = string.Empty;

    [Display(Name = "Мінуси")]
    public string? ReviewMinus { get; set; } = string.Empty;
    ////

    public Post()
    {
        UrlSlug = Id.ToString();
    }

    public Post(PostType type, string title, string content, string author)
    {
        UrlSlug = Id.ToString();
        TypeId = type;
        Title = title;
        Content = content;
        Author = author;
    }

    public Post(PostType type, string title, string content, string author, DateTime date)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Date = date;
    }

    public Post(PostType type, string title, string content, string author, string tags, int likes = 0, int dislikes = 0)
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Tags = tags;
        LikesCount = likes;
        DislikesCount = dislikes;
    }

    public Post(PostType type, string title, string content, string author, string tags, DateTime date, int likes = 0, int dislikes = 0, string imageUrl = "")
    : this(type, title, content, author)
    {
        UrlSlug = Id.ToString();
        Tags = tags;
        LikesCount = likes;
        DislikesCount = dislikes;
        Date = date;
        TitleImage = imageUrl;
    }

    public string GetColorCode(PostType typeId)
    {
#pragma warning disable IDE0066
        switch (typeId)
        {
            case PostType.Новина:
                return "blue";
            case PostType.Огляд:
                return "green";
            case PostType.Стаття:
                return "red";
            case PostType.Гайд:
                return "orange";
            case PostType.Відео:
                return "purple";
            case PostType.Подкаст:
                return "brown";
            default:
                return "black";
        }
#pragma warning restore IDE0066
    }
}

public enum PostType
{
    Новина,
    Огляд,
    Стаття,
    Гайд,
    Відео,
    Подкаст,
}
