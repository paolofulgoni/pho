using System.Text.Json.Serialization;

namespace Pho.Infrastructure.NasaNeo.Dto;

public class NasaNeoResponse
{
    [JsonPropertyName("near_earth_objects")]
    public Dictionary<DateOnly, List<NearEarthObject>>? NearEarthObjects { get; set; }
}
