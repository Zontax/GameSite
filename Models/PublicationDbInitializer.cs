﻿using GameSite.Data;

namespace GameSite.Models;

public class PublicationDbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Publications.Any())
        {
            //context.Publications.ExecuteDelete(); // Очистити БД
            return; // База даних ініційована
        }

        var publications = new Publication[]
        {
            new(Type.Новина, "Аміль Насіров переміг у номінації «найкращий український актор»",
                @"Під час сьомого фестивалю «Київський тиждень кіно» відбулася церемонія нагородження 
                за версією Національної премії кінокритиків «Кіноколо».",
                "Андрій Присяжний", 2, 0),
            new(Type.Новина, "Жіноча локалізаційна спілка CatLocTeam розширює команду. Долучайтесь!",
            @"Під час сьомого фестивалю «Київський тиждень кіно» відбулася церемонія нагородження за версією Національної премії кінокритиків «Кіноколо».",
            "Андрій Присяжний", 4, 0),
            new(Type.Новина, "Сім’я покійного актора дала згоду CD Projekt Red на відтворення його голосу з допомогою штучного інтелекту",
            @"CD Projekt Red використали штучний інтелект, щоб відтворити голос покійного польського актора озвучення Мілогоста Речека. Він виконував роль Віктора у Cyberpunk 2077.",
            "Дмитро Джугалик", 7, 2),
            new(Type.Новина, "Layers of Fear отримає безкоштовне сюжетне доповнення",
            @"Польська студія Bloober Team на своїй сторінці у Х (Twitter) поділилася приємною новиною для усіх шанувальників їхнього психологічного горору Layers of Fear ",
            "", 5, 0),
            new(Type.Новина, "Розробники Lords of the Fallen нізащо не використовуватимуть Denuvo",
            @"Польська студія Hexworks, яка розробила свіженький соулслайк Lords of the Fallen, висловила свою позицію стосовно популярної системи антипіратського захисту Denuvo. Цей DRM-захист є",
            "Олег Куліков", 5, 0),
            new(Type.Новина, "«Ходячі мерці» повертаються з новим серіалом про Ріка і Мішонн",
            @"Телеканал AMC анонсував новий серіал The Walking Dead: The Ones Who Live у всесвіті «Ходячих мерців» і продемонстрував перший тизер проєкту з головними героями. У серіалі велику увагу приділять таким персонажам, як Мішон і Рік Ґраймс, яких показали в короткому ролику під час боротьби зі зомбаками. Міні-серіал матиме всього 6 епізодів і вийде в лютому 2024 року",
            "Анна Колінчук", 7, 0),
            new(Type.Огляд, "Ну ось і все! Microsoft нарешті купує Activision Blizzard",
            @"Ми вже писали про те, що британське Управління з питань конкуренції та ринків переглянуло своє рішення стосовно угоди Microsoft і Activision Blizzard",
            "", 6, 0),
            new(Type.Огляд, "Симкада для народу — Огляд Forza Motorsport",
            @"Світ гонкових ігор потроху виходить із стагнації, і це чудово. За останніх десять років, поки “традиційні симулятори” на кшталт iRacing, Assetto Corsa",
            "Володимир Бортюк", 4, 2),
        };

        foreach (var p in publications)
        {
            context.Publications.Add(p);
        }

        context.SaveChanges();
    }
}

