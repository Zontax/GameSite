using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Author { get; set; }
    public DateTime Date { get; set; }
    public int LikesCount { get; set; }
    public int DislikesCount { get; set; }
    public string Tags { get; set; }
    public int GameId { get; set; }
    public ICollection<NewsComment> Comments { get; set; }

    public News(string title, string text, string author)
    {
        Title = title;
        Text = text;
        Author = author;
        Date = DateTime.Now;
        LikesCount = 0;
        DislikesCount = 0;
        Tags = string.Empty;
        GameId = 0;
    }

    public News(string title, string text, string author, DateTime date, int likes = 0, int dislikes = 0, string tags = "")
    {
        Title = title;
        Text = text;
        Author = author;
        Date = date;
        LikesCount = likes;
        DislikesCount = dislikes;
        Tags = tags;
        GameId = 0;
    }
}