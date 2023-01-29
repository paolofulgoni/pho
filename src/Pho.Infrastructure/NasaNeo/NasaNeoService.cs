using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pho.Core.Aggregates;
using Pho.Core.Exceptions;
using Pho.Core.Interfaces;
using Pho.Infrastructure.NasaNeo.Dto;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace Pho.Infrastructure.NasaNeo;

public class NasaNeoService : INearEarthAsteroidsService
{
    private const string RateLimitLimitHeader = "X-RateLimit-Limit";
    private const string RateLimitRemainingHeader = "X-RateLimit-Remaining";

    private readonly HttpClient _httpClient;
    private readonly NasaNeoServiceOptions _options;
    private readonly ILogger<NasaNeoService> _logger;

    public NasaNeoService(HttpClient httpClient, IOptions<NasaNeoServiceOptions> nasaNeoServiceOptions, ILogger<NasaNeoService> logger)
    {
        _httpClient = httpClient;
        _options = nasaNeoServiceOptions.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_options.BaseAddress);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<IEnumerable<Asteroid>> GetNearEarthAsteroids(DateOnly startDate, DateOnly endDate)
    {
        var requestUri = $"{_options.Endpoint}?" +
                         $"start_date={startDate:yyyy-MM-dd}&" +
                         $"end_date={endDate:yyyy-MM-dd}&" +
                         $"api_key={_options.ApiKey}";

        var httpResponse = await _httpClient.GetAsync(requestUri);

        LogRateLimitHeaders(httpResponse);

        switch (httpResponse.StatusCode)
        {
            case HttpStatusCode.TooManyRequests:
                throw new ThirdPartyServiceException("The Nasa NEO service rate limit has been exceeded.",
                    HttpStatusCode.TooManyRequests);
            case HttpStatusCode.Unauthorized:
            case HttpStatusCode.Forbidden:
                throw new ThirdPartyServiceException("The Nasa NEO service API key is invalid.",
                    HttpStatusCode.Unauthorized);
        }
        
        // In case of other unhandled error status codes
        if (!httpResponse.IsSuccessStatusCode)
            throw new ThirdPartyServiceException("The Nasa NEO service returned an unexpected error.",
                httpResponse.StatusCode);

        await using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var response = await JsonSerializer.DeserializeAsync<NasaNeoResponse>(responseStream) ??
                       throw new InvalidOperationException("The response from the Nasa NEO service is null");

        return MapToDomain(response);
    }

    private void LogRateLimitHeaders(HttpResponseMessage httpResponse)
    {
        httpResponse.Headers.TryGetValues(RateLimitLimitHeader, out var rateLimitLimit);
        httpResponse.Headers.TryGetValues(RateLimitRemainingHeader, out var rateLimitRemaining);

        _logger.LogInformation("Nasa NEO service rate limit: {RateLimitLimit}, remaining: {RateLimitRemaining}",
            rateLimitLimit, rateLimitRemaining);
    }

    private IEnumerable<Asteroid> MapToDomain(NasaNeoResponse response)
    {
        if (response.NearEarthObjects == null)
            return Enumerable.Empty<Asteroid>();

        return response.NearEarthObjects
            .SelectMany(nearEarthObjectsByDate => nearEarthObjectsByDate.Value)
            .Select(nearEarthObject => new Asteroid
            {
                Name = nearEarthObject.Name,
                EstimatedMinDiameter =
                    nearEarthObject.EstimatedDiameter[EstimatedDiameter.Kilometers].EstimatedDiameterMin,
                EstimatedMaxDiameter =
                    nearEarthObject.EstimatedDiameter[EstimatedDiameter.Kilometers].EstimatedDiameterMax,
                CloseApproachVelocity = double.Parse(
                    nearEarthObject.CloseApproachData[0].RelativeVelocity[CloseApproachData.KilometersPerHour],
                    CultureInfo.InvariantCulture),
                CloseApproachDate = nearEarthObject.CloseApproachData[0].CloseApproachDate,
                IsPotentiallyHazardous = nearEarthObject.IsPotentiallyHazardousAsteroid
            });
    }
}
