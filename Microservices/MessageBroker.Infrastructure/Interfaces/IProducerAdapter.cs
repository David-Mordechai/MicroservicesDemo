using MessageBroker.Core.Models;

namespace MessageBroker.Infrastructure.Interfaces;

internal interface IProducerAdapter
{
    Task<MessageBrokerResultModel> ProduceAsync(string topic, string message);
}