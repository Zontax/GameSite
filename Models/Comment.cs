using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int? ReplyCommentId { get; set; }
    //public string AuthorId { get; set; }
    [Display(Name = "Автор")]
    [Required(ErrorMessage = "Автор")]
    public string Author { get; set; }
    [Required(ErrorMessage = "Введіть текст до коментаря")]
    public string Text { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public bool Edited { get; set; }

    public Comment(int postId, string author, string text, bool edited = false, int? replyCommentId = null)
    {
        PostId = postId;
        ReplyCommentId = replyCommentId;
        Text = text;
        Author = author;
        Edited = edited;
    }
}