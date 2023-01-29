using Pho.Core.Aggregates;

namespace Pho.Web.ViewModels;

public class AsteroidViewModel
{
    /// <summary>
    /// Asteroid name
    /// </summary>
    public string Name { get; private init; } = string.Empty;
    
    /// <summary>
    /// Asteroid estimated diameter in kilometers
    /// </summary>
    public double Diameter { get; private init; }
    
    /// <summary>
    /// Asteroid approach velocity in kilometers per hour
    /// </summary>
    public double Velocity { get; private init; }
    
    /// <summary>
    /// Asteroid approach date
    /// </summary>
    public DateTimeOffset Date { get; private init; }
    
    public static AsteroidViewModel From(Asteroid asteroid) => new()
    {
        Name = asteroid.Name,
        Diameter = asteroid.GetAverageDiameter(),
        Velocity = asteroid.CloseApproachVelocity,
        Date = asteroid.CloseApproachDate
    };
}
