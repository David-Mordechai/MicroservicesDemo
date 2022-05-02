using MessageBroker.Core.Models;

namespace MessageBroker.Core;

public interface IPublisher
{
    Task<MessageResultModel> Publish<T>(T message, string topic) where T : class;
}