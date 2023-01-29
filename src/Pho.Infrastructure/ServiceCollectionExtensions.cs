using Microsoft.Extensions.DependencyInjection;
using Pho.Core.Interfaces;
using Pho.Infrastructure.NasaNeo;

namespace Pho.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNasaNeoService(this IServiceCollection services)
    {
        services.AddScoped<INearEarthAsteroidsService, NasaNeoService>();

        return services;
    }
}
