using Microsoft.Extensions.DependencyInjection;
using Pho.Core.Services;

namespace Pho.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IHazardousAsteroidsService, HazardousAsteroidsService>();

        return services;
    }
}
