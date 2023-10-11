using GameSite.Data;

namespace GameSite.Models;

public class NewsCommentDbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Перевірте, чи вже є данні в БД
        if (context.NewsComments.Any())
        {
            //context.Coments.ExecuteDelete(); // Очистити БД
            return; // База даних ініційована
        }

        var coments = new NewsComment[]
        {
            new(2, "Вадим", "Коментар 1"),
            new(2, "Ілля", "Коментар 1"),
            new(3, "Влад", "Коментар 1"),
            new(3, "Саша", "Коментар 1"),
        };

        foreach (var com in coments)
        {
            context.NewsComments.Add(com);
        }

        context.SaveChanges();
    }
}

