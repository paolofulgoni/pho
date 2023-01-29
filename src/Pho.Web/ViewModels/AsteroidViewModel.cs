using Pho.Core.Aggregates;

namespace Pho.Web.ViewModels;

public class AsteroidViewModel
{
    /// <summary>
    /// Asteroid name
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Asteroid estimated diameter in kilometers
    /// </summary>
    public double Diameter { get; init; }
    
    /// <summary>
    /// Asteroid approach velocity in kilometers per hour
    /// </summary>
    public double Velocity { get; init; }
    
    /// <summary>
    /// Asteroid approach date
    /// </summary>
    public DateTimeOffset Date { get; init; }
    
    public static AsteroidViewModel From(Asteroid asteroid) => new()
    {
        Name = asteroid.Name,
        Diameter = asteroid.Diameter,
        Velocity = asteroid.Velocity,
        Date = asteroid.Date
    };
}
