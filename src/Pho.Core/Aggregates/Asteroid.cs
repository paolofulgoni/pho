namespace Pho.Core.Aggregates;

public class Asteroid
{
    public string Name { get; set; }
    public int Diameter { get; set; }
    public float Velocity { get; set; }
    public DateTimeOffset Date { get; set; }
}
