namespace Pho.Infrastructure.NasaNeo;

public class NasaNeoServiceOptions
{
    public string BaseAddress { get; set; } = "https://api.nasa.gov";
    public string Endpoint { get; set; } = "neo/rest/v1/feed";
    public string ApiKey { get; set; } = string.Empty;
}
