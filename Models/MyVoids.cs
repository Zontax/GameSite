namespace GameSite.Models;

public static class MyVoids
{
    public static string GetColorCode(Type typeId)
    {
        switch (typeId)
        {
            case Type.Новина:
                return "rgb(201, 142, 5)";
            case Type.Огляд:
                return "orange";
            case Type.Стаття:
                return "yellow";
            case Type.Гайд:
                return "green";
            case Type.Відео:
                return "blue";
            case Type.Подкаст:
                return "brown";
            default:
                return "black";
        }
    }
}
