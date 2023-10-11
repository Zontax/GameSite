using System.ComponentModel.DataAnnotations;

namespace GameSite.Models;

public class Coment
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Автор обов'язковий")]
    public string Author { get; set; }

    [Required(ErrorMessage = "Текст обов'язковий")]
    public string Text { get; set; }

    [Display(Name = "Дата та час")]
    public DateTime Date { get; set; }

    public int PublicationId { get; set; }
    public Publication Publication { get; set; }

    public Coment(int publicationId, string author, string text)
    {
        PublicationId = publicationId;
        Text = text;
        Author = author;
        Date = DateTime.Now;
    }

    public Coment(int publicationId, string author, string text, DateTime date)
    {
        PublicationId = publicationId;
        Text = text;
        Author = author;
        Date = date;
    }
}