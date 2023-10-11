namespace GameSite.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Game(string name)
    {
        Name = name;
    }
}