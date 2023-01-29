namespace Pho.Core.Aggregates;

public class Asteroid
{
    public string Name { get; init; }
    public double Diameter { get; init; }
    public double Velocity { get; init; }
    public DateTimeOffset Date { get; init; }
}
