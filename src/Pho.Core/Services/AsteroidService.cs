using Pho.Core.Aggregates;

namespace Pho.Core.Services;

public interface IAsteroidService
{
    Task<IEnumerable<Asteroid>> GetPotentiallyHazardousAsteroids(int days);
}

public class AsteroidService : IAsteroidService
{
    public Task<IEnumerable<Asteroid>> GetPotentiallyHazardousAsteroids(int days)
    {
        // TODO
        return Task.FromResult<IEnumerable<Asteroid>>(new[] {new Asteroid()});
    }
}
