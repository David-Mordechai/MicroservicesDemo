using MessageBroker.Infrastructure;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using Microsoft.AspNetCore.Http.Connections;
using NotificationsService;
using NotificationsService.EventHandlers;
using NotificationsService.EventHandlers.Interfaces;
using NotificationsService.Hubs;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddMessageBrokerConsumerServicesRabbitMq(
        new RabbitMqConfiguration
        {
            BootstrapServers = builder.Configuration["brokerService"]
        });
    builder.Services.AddSingleton<INewMapPointEventHandler, NewMapPointEventHandler>();
    builder.Services.AddSingleton<INewMapEventHandler, NewMapEventHandler>();

    builder.Services.AddSignalR();

    builder.Services.AddSingleton<NotificationsSubscriber>();

    var app = builder.Build();

    app.MapHub<NotificationsHub>("/ws", options =>
    {
        options.Transports = HttpTransportType.WebSockets;
    });

    var notificationsSubscriber = app.Services.GetRequiredService<NotificationsSubscriber>();
    notificationsSubscriber.Subscribe();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

