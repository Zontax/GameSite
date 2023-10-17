namespace GameSite.Models;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Game(string name)
    {
        Name = name;
    }

    public Game(string name, string description)
    {
        Name = name;
        Description = description;
    }
}