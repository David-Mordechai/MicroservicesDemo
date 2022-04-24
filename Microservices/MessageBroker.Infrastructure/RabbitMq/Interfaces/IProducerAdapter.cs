using MessageBroker.Core.Models;

namespace MessageBroker.Infrastructure.RabbitMq.Interfaces;

internal interface IProducerAdapter
{
    Task<MessageResultModel> ProduceAsync(string topic, string message);
}