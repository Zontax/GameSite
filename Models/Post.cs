using System.ComponentModel.DataAnnotations;
using GameSite.Data;

namespace GameSite.Models;

public class Post
{
    public int Id { get; set; }

    [Display(Name = "Publication_type", ResourceType = typeof(Resources.Resource))]
    public PostType TypeId { get; set; } = PostType.Новина;

    [Required(ErrorMessageResourceType = typeof(Resources.Resource),
        ErrorMessageResourceName = "RequiredField")]
    [StringLength(140, ErrorMessageResourceType = typeof(Resources.Resource),
        ErrorMessageResourceName = "No_more_than_140_characters")]
    [Display(Name = "Title", ResourceType = typeof(Resources.Resource))]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Resources.Resource),
        ErrorMessageResourceName = "RequiredField")]
    [Display(Name = "Content", ResourceType = typeof(Resources.Resource))]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Publication_cover", ResourceType = typeof(Resources.Resource))]
    public string? TitleImage { get; set; } = string.Empty;

    public string? VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Resources.Resource),
        ErrorMessageResourceName = "RequiredField")]
    [Display(Name = "Author", ResourceType = typeof(Resources.Resource))]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string AuthorId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int LikesCount { get; set; }

    public virtual ICollection<ApplicationUser>? LikedByUsers { get; set; }

    public virtual ICollection<ApplicationUser>? SavedByUsers { get; set; }

    public int DislikesCount { get; set; }

    [Required(ErrorMessage = "Введіть теги")]
    [Display(Name = "AddTags", ResourceType = typeof(Resources.Resource))]
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

    [Display(Name = "Вдалося")]
    public string? ReviewPlus { get; set; } = string.Empty;

    [Display(Name = "Невдалося")]
    public string? ReviewMinus { get; set; } = string.Empty;
    ////

    public Post()
    {
    }

    public Post(string authorId)
    {
        AuthorId = authorId;
    }

    public Post(PostType type, string title, string content, string authorId, string author)
    {
        TypeId = type;
        Title = title;
        Content = content;
        Author = author;
        AuthorId = authorId;
    }

    public Post(PostType type, string title, string content, string authorId, string author, DateTime date)
    : this(type, title, content, authorId, author)
    {
        Date = date;
    }

    public Post(PostType type, string title, string content, string authorId, string author, string tags, int likes = 0, int dislikes = 0)
    : this(type, title, content, authorId, author)
    {
        Tags = tags;
        LikesCount = likes;
        DislikesCount = dislikes;
    }

    public Post(PostType type, string title, string content, string authorId, string author, string tags, DateTime date, int likes = 0, int dislikes = 0, string imageUrl = "")
    : this(type, title, content, authorId, author)
    {
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
