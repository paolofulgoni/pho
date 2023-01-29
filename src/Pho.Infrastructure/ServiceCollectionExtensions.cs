using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pho.Core.Interfaces;
using Pho.Infrastructure.NasaNeo;

namespace Pho.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNasaNeoService(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        services.Configure<NasaNeoServiceOptions>(configurationSection);

        services.AddHttpClient<INearEarthAsteroidsService, NasaNeoService>();

        return services;
    }
}
