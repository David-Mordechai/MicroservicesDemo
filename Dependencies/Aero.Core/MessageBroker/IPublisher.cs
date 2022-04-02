using Aero.Core.MessageBroker.Models;

namespace Aero.Core.MessageBroker;

public interface IPublisher
{
    Task<MessageResultModel> Publish<T>(T message, string topic) where T : class;
}