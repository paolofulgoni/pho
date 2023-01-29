using System.Text.Json.Serialization;

namespace Pho.Infrastructure.NasaNeo.Dto;

public class EstimatedDiameter
{
    public const string Kilometers = "kilometers";

    [JsonPropertyName("estimated_diameter_min")]
    public double EstimatedDiameterMin { get; set; }

    [JsonPropertyName("estimated_diameter_max")]
    public double EstimatedDiameterMax { get; set; }
}
