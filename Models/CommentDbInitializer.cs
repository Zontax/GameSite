using GameSite.Data;

namespace GameSite.Models;

public class CommentDbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Comments.Any())
        {
            //context.Coments.ExecuteDelete(); // Очистити БД
            return; // База даних ініційована
        }

        var comments = new Comment[]
        {
            new(2, "Вадим", "Коментар Вадим"),
            new(2, "Ілля", "Коментар Іллі"),
            new(3, "Влад", "Коментар Влада"),
            new(3, "Саша", "Коментар Саші"),
        };

        foreach (var comment in comments)
        {
            context.Comments.Add(comment);
        }

        context.SaveChanges();
    }
}

