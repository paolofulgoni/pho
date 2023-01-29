namespace Pho.Core.Aggregates;

public record Asteroid
{
    public string Name { get; init; } = string.Empty;
    public double EstimatedMinDiameter { get; init; }
    public double EstimatedMaxDiameter { get; init; }
    public double CloseApproachVelocity { get; init; }
    public DateOnly CloseApproachDate { get; init; }
    public bool IsPotentiallyHazardous { get; init; }

    public double GetAverageDiameter() => (EstimatedMinDiameter + EstimatedMaxDiameter) / 2;
}
