using System.Text.Json;
using Aero.Core.Logger;
using Aero.Core.MessageBroker;
using Aero.Core.MessageBroker.Models;
using Aero.Infrastructure.MessageBroker.RabbitMq.Interfaces;

namespace Aero.Infrastructure.MessageBroker;

internal class Publisher : IPublisher
{
    private readonly IAeroLogger<Publisher> _logger;
    private readonly IProducerAdapter _producer;

    public Publisher(
        IAeroLogger<Publisher> logger,
        IProducerAdapter producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task<MessageResultModel> Publish<T>(T message, string topic) where T : class
    {
        try
        {
            // todo extract serialization logic from here
            var messageAsJson = JsonSerializer.Serialize(message);
            
            var deliveryResult = await _producer.ProduceAsync(topic, messageAsJson);
            _logger.LogInformation($"Delivery success: {deliveryResult}");

            return deliveryResult;
        }
        catch (Exception e)
        {
            _logger.LogError($"Delivery failed: {e.Message}");
            return new MessageResultModel
            {
                Success = false,
                ErrorMessage = "Delivery failed."
            };
        }
    }
}