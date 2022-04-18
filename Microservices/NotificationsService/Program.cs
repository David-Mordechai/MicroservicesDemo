using MessageBroker.Infrastructure;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using Microsoft.AspNetCore.Http.Connections;
using NotificationsService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBrokerConsumerServicesRabbitMq(
    new RabbitMqConfiguration
    {
        BootstrapServers = "aero_rabbitmq"
    });
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<NotificationsHub>("/ws", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});

app.Run();