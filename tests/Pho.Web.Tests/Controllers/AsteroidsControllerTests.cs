using Pho.Core.Aggregates;
using Pho.Core.Services;
using Pho.Web.Controllers;

namespace Pho.Web.Tests;

public class AsteroidsControllerTests
{
    [Fact]
    public async Task Get_ReturnsAsteroidsInDescendingDiameterOrder_WhenAsteroidsExist()
    {
        // Arrange
        var smallAsteroid = new Asteroid {Name = "s", Diameter = 1.1, Velocity = 3.2, Date = DateTimeOffset.Now};
        var mediumAsteroid = new Asteroid {Name = "m", Diameter = 4.5, Velocity = 2.2, Date = DateTimeOffset.Now};
        var largeAsteroid = new Asteroid {Name = "l", Diameter = 9.3, Velocity = 4.2, Date = DateTimeOffset.Now};

        var asteroidServiceMock = new Mock<IAsteroidService>();
        asteroidServiceMock
            .Setup(m => m.GetLargestPotentiallyHazardousAsteroids(It.IsAny<int>()))
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
                first.Diameter.Should().Be(largeAsteroid.Diameter);
                first.Velocity.Should().Be(largeAsteroid.Velocity);
                first.Date.Should().Be(largeAsteroid.Date);
            },
            second =>
            {
                second.Name.Should().Be(mediumAsteroid.Name);
                second.Diameter.Should().Be(mediumAsteroid.Diameter);
                second.Velocity.Should().Be(mediumAsteroid.Velocity);
                second.Date.Should().Be(mediumAsteroid.Date);
            },
            third =>
            {
                third.Name.Should().Be(smallAsteroid.Name);
                third.Diameter.Should().Be(smallAsteroid.Diameter);
                third.Velocity.Should().Be(smallAsteroid.Velocity);
                third.Date.Should().Be(smallAsteroid.Date);
            });
    }
    
    
    [Fact]
    public async Task Get_ReturnsNoAsteroids_WhenAsteroidsDoNoExist()
    {
        // Arrange
        var asteroidServiceMock = new Mock<IAsteroidService>();
        asteroidServiceMock
            .Setup(m => m.GetLargestPotentiallyHazardousAsteroids(It.IsAny<int>()))
            .ReturnsAsync(Enumerable.Empty<Asteroid>());

        var controller = new AsteroidsController(asteroidServiceMock.Object);

        // Act
        var result = await controller.Get(7);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();
    }
}
