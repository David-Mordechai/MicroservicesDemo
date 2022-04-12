using Aero.Core.Logger;
using Aero.Core.MessageBroker;
using Aero.Infrastructure.Logger;
using Aero.Infrastructure.MessageBroker;
using Aero.Infrastructure.MessageBroker.RabbitMq;
using Aero.Infrastructure.MessageBroker.RabbitMq.Builder;
using Aero.Infrastructure.MessageBroker.RabbitMq.Builder.Configuration;
using Aero.Infrastructure.MessageBroker.RabbitMq.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Aero.Infrastructure;

public static class ServicesCollectionExtension
{
    public static void AddAeroInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IAeroLogger<>), typeof(AeroLogger<>));
    }

    public static void AddMessageBrokerProducerServicesRabbitMq(this IServiceCollection services,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        services.AddScoped<IPublisher, Publisher>();
        services.AddScoped(_ => new RabbitMqBuilderAdapter(rabbitMqConfiguration));
        services.AddScoped<IProducerAdapter, RabbitMqProducerAdapter>();
    }

    public static void AddMessageBrokerConsumerServicesRabbitMq(this IServiceCollection services,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        services.AddSingleton<ISubscriber, Subscriber>();
        services.AddSingleton(_ => new RabbitMqBuilderAdapter(rabbitMqConfiguration));
        services.AddSingleton<IConsumerAdapter, RabbitMqConsumerAdapter>();
    }
}