using System.Text.Json;
using MessageBroker.Core;
using MessageBroker.Core.Models;
using MessageBroker.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace MessageBroker.Infrastructure;

internal class Publisher : IPublisher
{
    private readonly ILogger<Publisher> _logger;
    private readonly IProducerAdapter _producer;

    public Publisher(
        ILogger<Publisher> logger,
        IProducerAdapter producer)
    {
        _logger = logger;
        _producer = producer;
    }

    public async Task<MessageBrokerResultModel> Publish<T>(T message, string topic) where T : class
    {
        try
        {
            var messageAsJson = JsonSerializer.Serialize(message);
            
            var deliveryResult = await _producer.ProduceAsync(topic, messageAsJson);
            _logger.LogInformation("Delivery success: {DeliveryResult}", deliveryResult.Message);

            return deliveryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Delivery failed: {ErrorMessage}", e.Message);
            return new MessageBrokerResultModel
            {
                Success = false,
                ErrorMessage = "Delivery failed."
            };
        }
    }
}