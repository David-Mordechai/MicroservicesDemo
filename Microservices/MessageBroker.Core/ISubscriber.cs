namespace MessageBroker.Core;

public interface ISubscriber
{
    void Subscribe(string topic, Func<string, 
        (bool success, string errorMessage)> consumeMessageHandler, CancellationToken cancellationToken);
}