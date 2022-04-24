using System.Text.Json;
using MessageBroker.Core;
using MessageBroker.Core.Models;
using MessageBroker.Infrastructure.RabbitMq.Interfaces;
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

    public async Task<MessageResultModel> Publish<T>(T message, string topic) where T : class
    {
        try
        {
            var messageAsJson = JsonSerializer.Serialize(message);
            
            var deliveryResult = await _producer.ProduceAsync(topic, messageAsJson);
            _logger.LogInformation("Delivery success: {deliveryResult}", deliveryResult);

            return deliveryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Delivery failed: {errorMessage}", e.Message);
            return new MessageResultModel
            {
                Success = false,
                ErrorMessage = "Delivery failed."
            };
        }
    }
}