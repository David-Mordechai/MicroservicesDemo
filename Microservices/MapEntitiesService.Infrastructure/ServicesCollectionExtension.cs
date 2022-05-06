using MapEntitiesService.Core.Configurations;
using MapEntitiesService.Core.Services;
using MapEntitiesService.Core.Services.Interfaces;
using MessageBroker.Infrastructure;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MapEntitiesService.Infrastructure;

public static class ServicesCollectionExtension
{
    public static void AddMapEntityServiceInfrastructure(this IServiceCollection services,
        Settings settings)
    {
        services.AddMessageBrokerProducerServicesRabbitMq(new RabbitMqConfiguration
        {
            BootstrapServers = settings.BrokerService
        });

        services.AddSingleton(settings);
        services.AddScoped<IMapEntityService, MapEntityService>();
    }
}