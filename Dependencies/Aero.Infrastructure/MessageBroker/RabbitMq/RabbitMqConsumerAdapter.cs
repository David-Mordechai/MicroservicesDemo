using System.Text;
using Aero.Infrastructure.MessageBroker.RabbitMq.Builder;
using Aero.Infrastructure.MessageBroker.RabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Aero.Infrastructure.MessageBroker.RabbitMq;

internal class RabbitMqConsumerAdapter : IConsumerAdapter, IDisposable
{
    private readonly RabbitMqBuilderAdapter _builderAdapter;
    private readonly IModel _channel;

    public RabbitMqConsumerAdapter(RabbitMqBuilderAdapter builderAdapter)
    {
        _builderAdapter = builderAdapter;
        _channel = builderAdapter.Build();
    }

    public void Subscribe(string topic, Action<string> consumeMessageHandler, CancellationToken cancellationToken)
    {
        try
        {
            _channel.ExchangeDeclare(exchange: topic, type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                exchange: topic,
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                consumeMessageHandler?.Invoke(message);
            };

            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);
        }
        catch (Exception ex)
        {
            throw ex switch
            {
                OperationCanceledException => new Exception("Operation was canceled."),
                _ => new Exception(ex.Message)
            };
        }
    }

    public void Dispose()
    {
        _builderAdapter.Dispose();
        GC.SuppressFinalize(this);
    }
}