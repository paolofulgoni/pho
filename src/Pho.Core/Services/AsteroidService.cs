using Pho.Core.Aggregates;

namespace Pho.Core.Services;

public interface IAsteroidService
{
    Task<IEnumerable<Asteroid>> GetLargestPotentiallyHazardousAsteroids(int days);
}

public class AsteroidService : IAsteroidService
{
    public Task<IEnumerable<Asteroid>> GetLargestPotentiallyHazardousAsteroids(int days)
    {
        // TODO
        
        var asteroids = new[]
        {
            new Asteroid
            {
                Name = "Asteroid 1",
                Diameter = 100,
                Velocity = 10,
                Date = DateTimeOffset.Now
            },
            new Asteroid
            {
                Name = "Asteroid 2",
                Diameter = 200,
                Velocity = 20,
                Date = DateTimeOffset.Now
            },
            new Asteroid
            {
                Name = "Asteroid 3",
                Diameter = 300,
                Velocity = 30,
                Date = DateTimeOffset.Now
            }
        };
        
        return Task.FromResult<IEnumerable<Asteroid>>(asteroids);
    }
}
