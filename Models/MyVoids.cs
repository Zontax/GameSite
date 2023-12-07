namespace GameSite.Models;

public static class MyVoids
{
    public static string GetColorCode(PostType typeId)
    {
        switch (typeId)
        {
            case PostType.Новина:
                return "rgb(201, 142, 5)";
            case PostType.Огляд:
                return "orange";
            case PostType.Стаття:
                return "yellow";
            case PostType.Гайд:
                return "green";
            case PostType.Відео:
                return "blue";
            case PostType.Подкаст:
                return "brown";
            default:
                return "black";
        }
    }
}
