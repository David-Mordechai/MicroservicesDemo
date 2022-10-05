using System.Text;
using MessageBroker.Infrastructure.Interfaces;
using MessageBroker.Infrastructure.RabbitMq.Builder;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageBroker.Infrastructure.RabbitMq;

internal class RabbitMqConsumerAdapter : IConsumerAdapter, IDisposable
{
    private readonly RabbitMqBuilderAdapter _builderAdapter;
    private readonly IModel _channel;

    public RabbitMqConsumerAdapter(RabbitMqBuilderAdapter builderAdapter)
    {
        _builderAdapter = builderAdapter;
        _channel = builderAdapter.Build();
    }

    public void Subscribe(string topic, Action<string>? consumeMessageHandler, CancellationToken cancellationToken)
    {
        try
        {
            _channel.QueueDeclare(queue: topic,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (_, ea) =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                consumeMessageHandler?.Invoke(message);
            };

            _channel.BasicConsume(queue: topic, autoAck: true, consumer: consumer);
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