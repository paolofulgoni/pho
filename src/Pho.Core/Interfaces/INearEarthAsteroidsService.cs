using Pho.Core.Aggregates;

namespace Pho.Core.Interfaces;

public interface INearEarthAsteroidsService
{
    public Task<IEnumerable<Asteroid>> GetNearEarthAsteroids(DateOnly startDate, DateOnly endDate);
}
