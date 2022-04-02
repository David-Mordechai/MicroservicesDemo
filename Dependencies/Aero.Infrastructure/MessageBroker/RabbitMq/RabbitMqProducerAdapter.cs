using System.Text;
using Aero.Core.MessageBroker.Models;
using Aero.Infrastructure.MessageBroker.RabbitMq.Builder;
using Aero.Infrastructure.MessageBroker.RabbitMq.Interfaces;
using RabbitMQ.Client;

namespace Aero.Infrastructure.MessageBroker.RabbitMq;

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

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: topic, routingKey: topic, basicProperties: null, messageBytes);
            _channel.QueueBind(queue: queueName,
                exchange: topic,
                routingKey: "");

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