namespace GameSite.Models;

public static class MyVoids
{
    public static string GetColorCode(PostType typeId)
    {
        return typeId switch
        {
            PostType.Новина => "rgb(201, 142, 5)",
            PostType.Огляд => "orange",
            PostType.Стаття => "yellow",
            PostType.Гайд => "green",
            PostType.Відео => "blue",
            PostType.Подкаст => "brown",
            _ => "black",
        };
    }
}
