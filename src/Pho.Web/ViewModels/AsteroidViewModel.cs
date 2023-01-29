using Pho.Core.Aggregates;

namespace Pho.Web.ViewModels;

/// <summary>
/// Asteroid
/// </summary>
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
    public DateOnly Date { get; private init; }
    
    /// <summary>
    /// Creates a new <see cref="AsteroidViewModel"/> from an <see cref="Asteroid"/>
    /// </summary>
    public static AsteroidViewModel From(Asteroid asteroid) => new()
    {
        Name = asteroid.Name,
        Diameter = asteroid.GetAverageDiameter(),
        Velocity = asteroid.CloseApproachVelocity,
        Date = asteroid.CloseApproachDate
    };
}
