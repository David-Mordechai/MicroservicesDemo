namespace MessageBroker.Infrastructure.RabbitMq.Interfaces;

internal interface IConsumerAdapter
{
    void Subscribe(string topic, Action<string> consumeMessageHandler, CancellationToken cancellationToken);
}