using System.Text.Json.Serialization;

namespace Pho.Infrastructure.NasaNeo.Dto;

public class NearEarthObject
{
    [JsonPropertyName("name")] public string Name { get; set; } = null!;

    [JsonPropertyName("estimated_diameter")]
    public Dictionary<string, EstimatedDiameter> EstimatedDiameter { get; set; } = null!;

    [JsonPropertyName("is_potentially_hazardous_asteroid")]
    public bool IsPotentiallyHazardousAsteroid { get; set; }

    [JsonPropertyName("close_approach_data")]
    public List<CloseApproachData> CloseApproachData { get; set; } = null!;
}
