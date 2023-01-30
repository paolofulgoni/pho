using Pho.Core.Aggregates;
using Pho.Core.Interfaces;
using Pho.Core.Services;

namespace Pho.Core.Tests.Services;

public class HazardousAsteroidsServiceTests
{
    [Fact]
    public async Task GetLargestHazardousAsteroids_ReturnsAsteroidsInDescendingDiameterOrder_WhenHazardousAsteroidsExist()
    {
        // Arrange
        var smallAsteroid = new Asteroid
        {
            Name = "s",
            EstimatedMinDiameter = 1.1,
            EstimatedMaxDiameter = 1.2,
            CloseApproachVelocity = 9.2,
            CloseApproachDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsPotentiallyHazardous = true
        };
        var mediumAsteroid = new Asteroid
        {
            Name = "m",
            EstimatedMinDiameter = 4.1,
            EstimatedMaxDiameter = 4.2,
            CloseApproachVelocity = 3.2,
            CloseApproachDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsPotentiallyHazardous = true
        };
        var largeAsteroid = new Asteroid
        {
            Name = "l",
            EstimatedMinDiameter = 16.1,
            EstimatedMaxDiameter = 18.2,
            CloseApproachVelocity = 6.2,
            CloseApproachDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsPotentiallyHazardous = true
        };
        var xlargeNonHazardousAsteroid = new Asteroid
        {
            Name = "l",
            EstimatedMinDiameter = 36.1,
            EstimatedMaxDiameter = 28.7,
            CloseApproachVelocity = 123.2333,
            CloseApproachDate = DateOnly.FromDateTime(DateTime.UtcNow),
            IsPotentiallyHazardous = false
        };

        var nearEarthAsteroidServiceMock = new Mock<INearEarthAsteroidsService>();
        nearEarthAsteroidServiceMock
            .Setup(m => m.GetNearEarthAsteroids(
                DateOnly.FromDateTime(DateTime.UtcNow),
                DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))))
            .ReturnsAsync(new List<Asteroid> {mediumAsteroid, smallAsteroid, largeAsteroid, xlargeNonHazardousAsteroid});

        var hazardousAsteroidsService = new HazardousAsteroidsService(nearEarthAsteroidServiceMock.Object);

        // Act
        var result = await hazardousAsteroidsService.GetLargestHazardousAsteroids(1, 2);

        // Assert
        result.Should().BeEquivalentTo(new [] {largeAsteroid, mediumAsteroid});
    }
}
