using Pho.Core.Aggregates;
using Pho.Core.Interfaces;

namespace Pho.Core.Services;

public interface IHazardousAsteroidsService
{
    Task<IEnumerable<Asteroid>> GetLargestHazardousAsteroids(int days, int maxCount);
}

public class HazardousAsteroidsService : IHazardousAsteroidsService
{
    private readonly INearEarthAsteroidsService _nearEarthAsteroidsService;

    public HazardousAsteroidsService(INearEarthAsteroidsService nearEarthAsteroidsService)
    {
        _nearEarthAsteroidsService = nearEarthAsteroidsService;
    }

    public async Task<IEnumerable<Asteroid>> GetLargestHazardousAsteroids(int days, int maxCount)
    {
        if (days < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(days), "Days must be greater than or equal to 0.");
        }
        if (maxCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxCount), "Max count must be greater than 0.");
        }

        var nearEarthAsteroids = await _nearEarthAsteroidsService.GetNearEarthAsteroids(
            startDate: DateOnly.FromDateTime(DateTime.UtcNow),
            endDate: DateOnly.FromDateTime(DateTime.UtcNow).AddDays(days));
        
        return nearEarthAsteroids
            .Where(asteroid => asteroid.IsPotentiallyHazardous)
            .OrderByDescending(asteroid => asteroid.GetAverageDiameter())
            .Take(maxCount);
    }
}
