using Pho.Core.Aggregates;
using Pho.Core.Services;
using Pho.Web.Controllers;

namespace Pho.Web.Tests;

public class AsteroidsControllerTests
{
    [Fact]
    public async Task Get_ReturnsAsteroidsInDescendingDiameterOrder_WhenHazardousAsteroidsExist()
    {
        // Arrange
        var smallAsteroid = new Asteroid
        {
            Name = "s",
            EstimatedMinDiameter = 1.1,
            EstimatedMaxDiameter = 1.2,
            CloseApproachVelocity = 9.2,
            CloseApproachDate = DateTimeOffset.Now,
            IsPotentiallyHazardous = true
        };
        var mediumAsteroid = new Asteroid
        {
            Name = "m",
            EstimatedMinDiameter = 4.1,
            EstimatedMaxDiameter = 4.2,
            CloseApproachVelocity = 3.2,
            CloseApproachDate = DateTimeOffset.Now,
            IsPotentiallyHazardous = true
        };
        var largeAsteroid = new Asteroid
        {
            Name = "l",
            EstimatedMinDiameter = 16.1,
            EstimatedMaxDiameter = 18.2,
            CloseApproachVelocity = 6.2,
            CloseApproachDate = DateTimeOffset.Now,
            IsPotentiallyHazardous = true
        };

        var asteroidServiceMock = new Mock<IHazardousAsteroidsService>();
        asteroidServiceMock
            .Setup(m => m.GetLargestHazardousAsteroids(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<Asteroid> {mediumAsteroid, smallAsteroid, largeAsteroid});

        var controller = new AsteroidsController(asteroidServiceMock.Object);

        // Act
        var result = await controller.Get(7);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Should().SatisfyRespectively(
            first =>
            {
                first.Name.Should().Be(largeAsteroid.Name);
                first.Diameter.Should().Be(largeAsteroid.GetAverageDiameter());
                first.Velocity.Should().Be(largeAsteroid.CloseApproachVelocity);
                first.Date.Should().Be(largeAsteroid.CloseApproachDate);
            },
            second =>
            {
                second.Name.Should().Be(mediumAsteroid.Name);
                second.Diameter.Should().Be(mediumAsteroid.GetAverageDiameter());
                second.Velocity.Should().Be(mediumAsteroid.CloseApproachVelocity);
                second.Date.Should().Be(mediumAsteroid.CloseApproachDate);
            },
            third =>
            {
                third.Name.Should().Be(smallAsteroid.Name);
                third.Diameter.Should().Be(smallAsteroid.GetAverageDiameter());
                third.Velocity.Should().Be(smallAsteroid.CloseApproachVelocity);
                third.Date.Should().Be(smallAsteroid.CloseApproachDate);
            });
    }


    [Fact]
    public async Task Get_ReturnsNoAsteroids_WhenNoHazardousAsteroidsExist()
    {
        // Arrange
        var asteroidServiceMock = new Mock<IHazardousAsteroidsService>();
        asteroidServiceMock
            .Setup(m => m.GetLargestHazardousAsteroids(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(Enumerable.Empty<Asteroid>());

        var controller = new AsteroidsController(asteroidServiceMock.Object);

        // Act
        var result = await controller.Get(7);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }
}
