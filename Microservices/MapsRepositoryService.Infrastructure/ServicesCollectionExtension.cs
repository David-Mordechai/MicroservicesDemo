using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Infrastructure.MinIo;
using MapsRepositoryService.Infrastructure.MinIo.Configuration;
using MapsRepositoryService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MapsRepositoryService.Infrastructure;

public static class ServicesCollectionExtension
{
    public static void AddMapsRepositoryServiceInfrastructure(this IServiceCollection services, MinIoConfiguration minIoConfiguration)
    {
        services.AddScoped<IMapsRepository, MapsRepository>();
        services.AddScoped(_ => minIoConfiguration);
        services.AddScoped<IMinIoClientBuilder, MinIoClientBuilder>();
    }
}