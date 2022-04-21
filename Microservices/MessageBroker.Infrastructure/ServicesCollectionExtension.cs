using MessageBroker.Core;
using MessageBroker.Infrastructure.RabbitMq;
using MessageBroker.Infrastructure.RabbitMq.Builder;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using MessageBroker.Infrastructure.RabbitMq.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBroker.Infrastructure;

public static class ServicesCollectionExtension
{
    public static void AddMessageBrokerProducerServicesRabbitMq(this IServiceCollection services,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        services.AddSingleton<IPublisher, Publisher>();
        services.AddSingleton(_ => new RabbitMqBuilderAdapter(rabbitMqConfiguration));
        services.AddSingleton<IProducerAdapter, RabbitMqProducerAdapter>();
    }

    public static void AddMessageBrokerConsumerServicesRabbitMq(this IServiceCollection services,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        services.AddSingleton<ISubscriber, Subscriber>();
        services.AddSingleton(_ => new RabbitMqBuilderAdapter(rabbitMqConfiguration));
        services.AddSingleton<IConsumerAdapter, RabbitMqConsumerAdapter>();
    }
}