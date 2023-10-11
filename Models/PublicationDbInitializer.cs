﻿using GameSite.Data;

namespace GameSite.Models;

public class PublicationDbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Перевірте, чи вже є данні в БД
        if (context.Publications.Any())
        {
            //context.Publications.ExecuteDelete(); // Очистити БД
            return; // База даних ініційована
        }

        // Додайте початкові данні в БД
        var publications = new Publication[]
        {
            new("Поради для збереження здоров'я",
                "Збереження здоров'я важливо для всіх. Ось кілька порад щодо правильного харчування та активного способу життя...",
                "Лікар Іванова"),
            new("Вивчення нових технологій програмування",
                "Якщо ви цікавитесь програмуванням, ось кілька рекомендацій щодо вивчення нових мов програмування та інструментів розробки...",
                "Розробник Петров"),
            new("Ідеї для відпочинку на вихідних",
                "Ви втомилися від роботи та шукаєте способи розваги на вихідних? Ось кілька цікавих ідей для активного відпочинку...",
                "Активний відпочивальник"),
            new("Кращі книги для літнього читання",
                "Літо - ідеальний час для читання. Ось кілька захоплюючих книг у різних жанрах, які варто спробувати цього літа...",
                "Книголюб"),
            new("Тренди у світі мистецтва",
                "Штучний інтелект, віртуальна реальність і інші технології змінюють мистецтво. Дізнайтеся про головні тренди у світі сучасного мистецтва...",
                "Митець-Художник"),
            new("Секрети успішних стартапів",
                "Якщо ви плануєте розпочати власний бізнес, ці поради та секрети успіху можуть бути корисними для вас у вашому підприємницькому шляху...",
                "Бізнесменка Сидоренко")
        };

        foreach (var publication in publications)
        {
            context.Publications.Add(publication);
        }

        context.SaveChanges();
    }
}
