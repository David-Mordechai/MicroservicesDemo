using MessageBroker.Core.Models;

namespace MessageBroker.Core;

public interface IPublisher
{
    Task<MessageBrokerResultModel> Publish<T>(T message, string topic) where T : class;
}