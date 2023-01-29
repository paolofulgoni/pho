using Pho.Core.Aggregates;

namespace Pho.Web.ViewModels;

public class AsteroidViewModel
{
    public string Name { get; set; }
    public int Diameter { get; set; }
    public float Velocity { get; set; }
    public DateTimeOffset Date { get; set; }
    
    public static AsteroidViewModel From(Asteroid asteroid) => new()
    {
        Name = asteroid.Name,
        Diameter = asteroid.Diameter,
        Velocity = asteroid.Velocity,
        Date = asteroid.Date
    };
}
