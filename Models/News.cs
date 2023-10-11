using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Publication
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Назва обов'язкова")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Текст обов'язковий")]
    public string Text { get; set; }

    [Required(ErrorMessage = "Автор обов'язковий")]
    public string Author { get; set; }

    [Display(Name = "Дата та час")]
    public DateTime Date { get; set; }

    public int? GameId { get; set; }

    public Publication(string title, string text, string author)
    {
        Title = title;
        Text = text;
        Author = author;
        Date = DateTime.Now;
    }

    public Publication(string title, string text, string author, DateTime date)
    {
        Title = title;
        Text = text;
        Author = author;
        Date = date;
    }
}