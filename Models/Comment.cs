using System.ComponentModel.DataAnnotations;
using GameSite.Data;

namespace GameSite.Models;

public class Comment
{
    public int Id { get; set; }
    public Post? Post { get; set; }
    public int PostId { get; set; }
    public int? ReplyCommentId { get; set; }
    public ApplicationUser? Author { get; set; }
    public string AuthorId { get; set; }
    [Required(ErrorMessage = "Введіть текст до коментаря")]
    public string? Text { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public bool Edited { get; set; }

    public Comment(int postId, string authorId, string text, bool edited = false, int? replyCommentId = null)
    {
        PostId = postId;
        ReplyCommentId = replyCommentId;
        Text = text;
        AuthorId = authorId;
        Edited = edited;
    }
}
