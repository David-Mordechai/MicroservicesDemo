using System.Text;
using MessageBroker.Core.Models;
using MessageBroker.Infrastructure.Interfaces;
using MessageBroker.Infrastructure.RabbitMq.Builder;
using RabbitMQ.Client;

namespace MessageBroker.Infrastructure.RabbitMq;

internal class RabbitMqProducerAdapter : IProducerAdapter, IDisposable
{
    private readonly RabbitMqBuilderAdapter _rabbitMqBuilder;
    private readonly IModel _channel;

    public RabbitMqProducerAdapter(RabbitMqBuilderAdapter rabbitMqBuilder)
    {
        _rabbitMqBuilder = rabbitMqBuilder;
        _channel = rabbitMqBuilder.Build();
    }

    public Task<MessageResultModel> ProduceAsync(string topic, string message)
    {
        try
        {
            _channel.QueueDeclare(queue: topic,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: null, messageBytes);

            return Task.FromResult(new MessageResultModel
            {
                Message = $"Message: '{message}' delivered to clients",
                Success = true
            });
        }
        catch (Exception ex)
        {
            throw ex switch
            {
                _ => new Exception(ex.Message)
            };
        }
    }

    public void Dispose()
    {
        _rabbitMqBuilder.Dispose();
        GC.SuppressFinalize(this);
    }
}