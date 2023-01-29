using Pho.Core.Aggregates;
using Pho.Core.Interfaces;

namespace Pho.Infrastructure.NasaNeo;

public class NasaNeoService : INearEarthAsteroidsService
{
    public Task<IEnumerable<Asteroid>> GetNearEarthAsteroids(DateOnly startDate, DateOnly endDate)
    {
        // TODO
        IEnumerable<Asteroid> asteroids = new List<Asteroid>
        {
            new()
            {
                Name = "Asteroid 1",
                EstimatedMinDiameter = 1,
                EstimatedMaxDiameter = 2,
                CloseApproachVelocity = 3,
                CloseApproachDate = DateTimeOffset.UtcNow,
                IsPotentiallyHazardous = true
            },
            new()
            {
                Name = "Asteroid 2",
                EstimatedMinDiameter = 2,
                EstimatedMaxDiameter = 3,
                CloseApproachVelocity = 4,
                CloseApproachDate = DateTimeOffset.UtcNow,
                IsPotentiallyHazardous = true
            },
            new()
            {
                Name = "Asteroid 3",
                EstimatedMinDiameter = 3,
                EstimatedMaxDiameter = 4,
                CloseApproachVelocity = 5,
                CloseApproachDate = DateTimeOffset.UtcNow,
                IsPotentiallyHazardous = false
            },
            new()
            {
                Name = "Asteroid 4",
                EstimatedMinDiameter = 4,
                EstimatedMaxDiameter = 5,
                CloseApproachVelocity = 6,
                CloseApproachDate = DateTimeOffset.UtcNow,
                IsPotentiallyHazardous = true
            },
            new()
            {
                Name = "Asteroid 5",
                EstimatedMinDiameter = 5,
                EstimatedMaxDiameter = 6,
                CloseApproachVelocity = 7,
                CloseApproachDate = DateTimeOffset.UtcNow,
                IsPotentiallyHazardous = true
            }
        };
            
        return Task.FromResult(asteroids);
    }
}
