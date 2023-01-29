using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq.Protected;
using Pho.Infrastructure.NasaNeo;
using System.Net;
using System.Reflection;

namespace Pho.Infrastructure.Tests.NasaNeo;

public class NasaNeoServiceTests
{
    [Fact]
    public async Task GetNearEarthAsteroids_ReturnsValidData_WhenThirdPartyServiceResponseIsOk()
    {
        // Arrange
        var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var content = File.ReadAllText(Path.Combine(currentDirectory, "TestData", "nasa-neo-response.json"));

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK, Content = new StringContent(content),
            })
            .Verifiable();

        var clientMock = new HttpClient(handlerMock.Object);

        var optionsMock = new Mock<IOptions<NasaNeoServiceOptions>>();
        optionsMock
            .SetupGet(o => o.Value)
            .Returns(new NasaNeoServiceOptions());

        var service = new NasaNeoService(clientMock, optionsMock.Object, NullLogger<NasaNeoService>.Instance);

        // Act
        var result = await service.GetNearEarthAsteroids(
            startDate: DateOnly.FromDateTime(DateTime.UtcNow),
            endDate: DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));

        // Assert
        result.Should().NotBeNull();
        result.Should().SatisfyRespectively(
            first =>
            {
                first.Name.Should().Be("68359 (2001 OZ13)");
                first.EstimatedMinDiameter.Should().BeApproximately(0.6707411893, 0.001);
                first.EstimatedMaxDiameter.Should().BeApproximately(1.4998228946, 0.001);
                first.CloseApproachVelocity.Should().BeApproximately(17093.5130926513, 0.1);
                first.CloseApproachDate.Should().Be(new DateOnly(2023, 01, 30));
                first.IsPotentiallyHazardous.Should().BeFalse();
            },
            second =>
            {
                second.Name.Should().Be("(2009 BH2)");
                second.EstimatedMinDiameter.Should().BeApproximately(0.0840533402, 0.001);
                second.EstimatedMaxDiameter.Should().BeApproximately(0.1879489824, 0.001);
                second.CloseApproachVelocity.Should().BeApproximately(49048.03006331, 0.001);
                second.CloseApproachDate.Should().Be(new DateOnly(2023, 01, 29));
                second.IsPotentiallyHazardous.Should().BeTrue();
            });
    }

    [Fact]
    public void GetNearEarthAsteroids_ThrowsHttpRequestException_WhenThirdPartyServiceRateLimited()
    {
        // Arrange

        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.TooManyRequests, Content = new StringContent("OVER_RATE_LIMIT"),
            })
            .Verifiable();

        var clientMock = new HttpClient(handlerMock.Object);

        var optionsMock = new Mock<IOptions<NasaNeoServiceOptions>>();
        optionsMock
            .SetupGet(o => o.Value)
            .Returns(new NasaNeoServiceOptions());

        var service = new NasaNeoService(clientMock, optionsMock.Object, NullLogger<NasaNeoService>.Instance);

        // Act
        Func<Task> act = async () =>
        {
            await service.GetNearEarthAsteroids(
                startDate: DateOnly.FromDateTime(DateTime.UtcNow),
                endDate: DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
        };

        // Assert
        act.Should().ThrowAsync<HttpRequestException>().Where(e => e.StatusCode == HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public void GetNearEarthAsteroids_ThrowsHttpRequestException_WhenThirdPartyServiceUnauthorized()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Forbidden, Content = new StringContent("API_KEY_MISSING"),
            })
            .Verifiable();

        var clientMock = new HttpClient(handlerMock.Object);

        var optionsMock = new Mock<IOptions<NasaNeoServiceOptions>>();
        optionsMock
            .SetupGet(o => o.Value)
            .Returns(new NasaNeoServiceOptions());

        var service = new NasaNeoService(clientMock, optionsMock.Object, NullLogger<NasaNeoService>.Instance);

        // Act
        Func<Task> act = async () =>
        {
            await service.GetNearEarthAsteroids(
                startDate: DateOnly.FromDateTime(DateTime.UtcNow),
                endDate: DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1));
        };

        // Assert
        act.Should().ThrowAsync<HttpRequestException>().Where(e => e.StatusCode == HttpStatusCode.Unauthorized);
    }
}
