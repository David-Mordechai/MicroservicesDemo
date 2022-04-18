using System.Text;
using MessageBroker.Core.Models;
using MessageBroker.Infrastructure.RabbitMq.Builder;
using MessageBroker.Infrastructure.RabbitMq.Interfaces;
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
            _channel.ExchangeDeclare(exchange: topic, type: ExchangeType.Fanout);
            var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                exchange: topic,
                routingKey: "");
            
            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: topic, routingKey: topic, basicProperties: null, messageBytes);

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