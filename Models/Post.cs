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
    public string? TitleImage { get; set; } = null;

    public string? VideoUrl { get; set; } = null;

    [Required(ErrorMessageResourceType = typeof(Resources.Resource),
        ErrorMessageResourceName = "RequiredField")]
    [Display(Name = "Author", ResourceType = typeof(Resources.Resource))]
    public string Author { get; set; } = string.Empty;

    [Required]
    public string? AuthorId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int LikesCount { get; set; }

    public int DislikesCount { get; set; }

    public virtual ICollection<ApplicationUser>? LikedByUsers { get; set; }

    public virtual ICollection<ApplicationUser>? DislikedByUsers { get; set; }

    public virtual ICollection<ApplicationUser>? SavedByUsers { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources.Resource),
        ErrorMessageResourceName = "SetTags")]
    [Display(Name = "AddTags", ResourceType = typeof(Resources.Resource))]
    public string Tags { get; set; } = string.Empty;

    [Display(Name = "Чи редаговано")]
    public bool Edited { get; set; } = false;

    [Display(Name = "Дата редагування")]
    public DateTime EditedDate { get; set; } = DateTime.UtcNow;

    [Display(Name = "ReviewRating", ResourceType = typeof(Resources.Resource))]
    public int? ReviewRating { get; set; }

    [Display(Name = "ReviewPlus", ResourceType = typeof(Resources.Resource))]
    public string? ReviewPlus { get; set; } = string.Empty;

    [Display(Name = "ReviewMinus", ResourceType = typeof(Resources.Resource))]
    public string? ReviewMinus { get; set; } = string.Empty;

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
        return typeId switch
        {
            PostType.Новина => "blue",
            PostType.Огляд => "green",
            PostType.Стаття => "red",
            PostType.Гайд => "orange",
            _ => "black",
        };
    }
}

public enum PostType
{
    Новина,
    Огляд,
    Стаття,
    Гайд
}
