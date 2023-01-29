using System.Text.Json.Serialization;

namespace Pho.Infrastructure.NasaNeo.Dto;

public class CloseApproachData
{
    public const string KilometersPerHour = "kilometers_per_hour";

    [JsonPropertyName("close_approach_date")]
    public DateOnly CloseApproachDate { get; set; }

    [JsonPropertyName("relative_velocity")]
    public Dictionary<string, string> RelativeVelocity { get; set; } = null!;
}
