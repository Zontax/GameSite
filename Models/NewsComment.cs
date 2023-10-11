namespace GameSite.Models;

public class NewsComment
{
    public int Id { get; set; }
    public string Author { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public int NewsId { get; set; }

    public NewsComment(int newsId, string author, string text)
    {
        NewsId = newsId;
        Text = text;
        Author = author;
        Date = DateTime.Now;
    }

    public NewsComment(int newsId, string author, string text, DateTime date)
    {
        NewsId = newsId;
        Text = text;
        Author = author;
        Date = date;
    }
}