using Aero.Core.Logger;
using Aero.Core.MessageBroker;
using Aero.Infrastructure.MessageBroker.RabbitMq.Interfaces;

namespace Aero.Infrastructure.MessageBroker;

internal class Subscriber : ISubscriber
{
    private readonly IAeroLogger<Subscriber> _logger;
    private readonly IConsumerAdapter _consumer;

    public Subscriber(IAeroLogger<Subscriber> logger, IConsumerAdapter consumer)
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
                    _logger.LogError($"Fail to process message, {errorMessage}");
            }, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}