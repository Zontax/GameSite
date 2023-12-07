using GameSite.Data;
using Microsoft.AspNetCore.Identity;

namespace GameSite.Models;

public class PostDbInitializer
{
    public static async void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (context.Posts.Any())
        {
            // context.Posts.ExecuteDelete();
            // context.Comments.ExecuteDelete();
            // context.SaveChanges();
            return;
        }

        List<Post> posts = new()
        {
             new (PostType.Новина,
                "S.T.A.L.K.E.R. 2: Серце Чорнобиля отримала новий трейлер! І стала найочікуванішою грою на PC Gaming Show",
                @"<p>Українська гра S.T.A.L.K.E.R. 2: Серце Чорнобиля від GSC Game World була обрана з-понад 130 проектів як найочікуваніша гра року на PC Gaming Show. 
	З цієї нагоди Євген Григорович, генеральний директор студії і керівник розробки S.T.A.L.K.E.R. 2: Heart of Chornobyl, звернувся до глядачів і подякував усім шанувальникам за підтримку, адже ця нагорода стала справжнім «символом непохитної довіри».</p>
	<iframe width=""560"" height=""315"" src=""https://www.youtube.com/embed/XNGbS0fY80s"" frameborder=""0"" allowfullscreen></iframe>
<p>Студія взялася за розробку сиквелу історії, що мала завершитися ще багато років тому, через незгасний інтерес відданих фанатів, які не захотіли залишати Зону й продовжували наповнювати її своїми розповідями.</p>
<img src="""" alt=""Зображення"">",
                "Андрій Присяжний",
                "GSC_Game_Worl, Hear_of_Chornobyl, S.T.A.L.K.E.R._2, STALKER, STALKER_2, бродяга, Григорович, зона, ігри_українською, локалізація, нагорода, найочікуваніша_гра, Серце_Чорнобиля, сюжетний_трейлер, українська_гра, Чорнобиль",
                DateTime.Now, 26, 2, "/post_title_image/86751ea5-6341-4aea-850e-10e8039dc5f4.png"),
            new(PostType.Новина, "Жіноча локалізаційна спілка CatLocTeam розширює команду. Долучайтесь!",
            @"Під час сьомого фестивалю «Київський тиждень кіно» відбулася церемонія нагородження за версією Національної премії кінокритиків «Кіноколо».",
            "Андрій Присяжний", "localisation, актор, studio", DateTime.Now.AddDays(-1), 4, 0),
            new(PostType.Новина, "Сім’я покійного актора дала згоду CD Projekt Red на відтворення його голосу з допомогою штучного інтелекту",
            @"CD Projekt Red використали штучний інтелект, щоб відтворити голос покійного польського актора озвучення Мілогоста Речека. Він виконував роль Віктора у Cyberpunk 2077.",
            "Дмитро Джугалик", "cd_pj_red, голос, ai", DateTime.Now.AddDays(-1), 7, 2),
            new(PostType.Новина, "Layers of Fear отримає безкоштовне сюжетне доповнення",
            @"Польська студія Bloober Team на своїй сторінці у Х (Twitter) поділилася приємною новиною для усіх шанувальників їхнього психологічного горору Layers of Fear ",
            "Алекс Войницький", "layers, актор", DateTime.Now.AddDays(-1), 5, 0),
            new(PostType.Новина, "Розробники Lords of the Fallen нізащо не використовуватимуть Denuvo",
            @"Польська студія Hexworks, яка розробила свіженький соулслайк Lords of the Fallen, висловила свою позицію стосовно популярної системи антипіратського захисту Denuvo. Цей DRM-захист є",
            "Олег Куліков", "poland", DateTime.Now.AddDays(-1), 5, 0),
            new(PostType.Новина, "«Ходячі мерці» повертаються з новим серіалом про Ріка і Мішонн",
            @"Телеканал AMC анонсував новий серіал The Walking Dead: The Ones Who Live у всесвіті «Ходячих мерців» і продемонстрував перший тизер проєкту з головними героями. У серіалі велику увагу приділять таким персонажам, як Мішон і Рік Ґраймс, яких показали в короткому ролику під час боротьби зі зомбаками. Міні-серіал матиме всього 6 епізодів і вийде в лютому 2024 року",
            "Анна Колінчук", "walking_dead, zombie", DateTime.Now.AddDays(-1), 7, 0),
            new(PostType.Огляд, "Ну ось і все! Microsoft нарешті купує Activision Blizzard",
            @"Ми вже писали про те, що британське Управління з питань конкуренції та ринків переглянуло своє рішення стосовно угоди Microsoft і Activision Blizzard",
            "Автор", "актор, що", DateTime.Now.AddDays(-1), 6, 0),
            new(PostType.Огляд, "Симкада для народу — Огляд Forza Motorsport",
            @"Світ гонкових ігор потроху виходить із стагнації, і це чудово. За останніх десять років, поки “традиційні симулятори” на кшталт iRacing, Assetto Corsa",
            "Володимир Бортюк", "forza_motosport, drive, auto", DateTime.Now.AddDays(-1), 4, 2),
        };

        List<Comment> comments = new()
        {
            new(1, "Вадим Горішник", "Мені сподобався ваш перший сайт на asp mvc", true),
            new(1, "Ілля Теркан", "Так, непогано як для першого проекта", false, 1),
            new(1, "Vadimchik", "The best info", true),
            new(1, "Ждан", "Хай додасть більше функцій для ствовення публікацій", true, 1),

            new(2, "Vlad", "Cool story", true),
            new(2, "Вадим", "Коментар Вадим", true),
            new(2, "Ілля", "Коментар Іллі", true),
            new(3, "Влад", "Коментар Влада"),
            new(3, "Саша", "Коментар Саші"),
        };

        context.Posts.AddRange(posts);
        context.Comments.AddRange(comments);


        var role1 = new IdentityRole { Name = "Admin" };
        var role2 = new IdentityRole { Name = "Author" };
        var role3 = new IdentityRole { Name = "Manager" };

        if (!context.Roles.Any())
        {
            await roleManager.CreateAsync(role1);
            await roleManager.CreateAsync(role2);
            await roleManager.CreateAsync(role3);
        }

        if (!context.Users.Any())
        {
            var adminUser = new ApplicationUser { UserName = "jrvadim18@gmail.com", Email = "jrvadim18@gmail.com" };
            adminUser.Name = "АДМІН";
            adminUser.RegistrationDate = DateTime.Now;
            adminUser.EmailConfirmed = true;

            var result = await userManager.CreateAsync(adminUser, "asd123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        context.SaveChanges();
    }
}

