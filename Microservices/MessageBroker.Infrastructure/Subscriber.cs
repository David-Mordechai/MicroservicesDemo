using MessageBroker.Core;
using MessageBroker.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace MessageBroker.Infrastructure;

internal class Subscriber : ISubscriber
{
    private readonly ILogger<Subscriber> _logger;
    private readonly IConsumerAdapter _consumer;

    public Subscriber(ILogger<Subscriber> logger, IConsumerAdapter consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    public void Subscribe(string topic, 
        Func<string, (bool success, string errorMessage)> consumeMessageHandler,
        CancellationToken cancellationToken)
    {
        try
        {
            _consumer.Subscribe(topic, consumeMessage =>
            {
                var (success, errorMessage) = consumeMessageHandler.Invoke(consumeMessage);
                if (success is false)
                    _logger.LogError("Fail to process message, {ErrorMessage}", errorMessage);
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Subscriber.Subscribe failed: {ErrorMessage}", e.Message);
        }
    }
}