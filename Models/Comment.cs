using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Comment
{
    public int Id { get; set; }
    public int PublicationId { get; set; }
    public int? ReplyCommentId { get; set; }
    [Display(Name = "Автор")]
    public string Author { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public bool Edited { get; set; }

    public Comment(int publicationId, string author, string text, int? replyCommentId = null)
    {
        if (replyCommentId == null)
        {
            PublicationId = publicationId;
        }
        else
        {
            ReplyCommentId = replyCommentId;
        }

        Text = text;
        Author = author;
        Date = DateTime.Now;
    }
}