using GameSite.Data;

namespace GameSite.Models;

public class ComentDbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Перевірте, чи вже є данні в БД
        if (context.Coments.Any())
        {
            //context.Coments.ExecuteDelete(); // Очистити БД
            return; // База даних ініційована
        }

        // Додайте початкові данні в БД
        var coments = new Coment[]
        {
            new(2, "Вадим", "Коментар 1"),
            new(2, "Ілля", "Коментар 1"),
            new(3, "Влад", "Коментар 1"),
            new(3, "Саша", "Коментар 1"),
        };

        foreach (var com in coments)
        {
            context.Coments.Add(com);
        }

        context.SaveChanges();
    }
}

