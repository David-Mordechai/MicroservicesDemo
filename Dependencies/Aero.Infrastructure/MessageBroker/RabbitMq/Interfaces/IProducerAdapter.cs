using Aero.Core.MessageBroker.Models;

namespace Aero.Infrastructure.MessageBroker.RabbitMq.Interfaces;

internal interface IProducerAdapter
{
    Task<MessageResultModel> ProduceAsync(string topic, string message);
}