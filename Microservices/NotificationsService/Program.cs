using MessageBroker.Infrastructure;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using Microsoft.AspNetCore.Http.Connections;
using NotificationsService;
using NotificationsService.Commands;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;
using NotificationsService.Hubs;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSignalR();
    builder.Host.UseSerilog((builderContext, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console()
        .ReadFrom.Configuration(builderContext.Configuration));

    var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
    builder.Services.AddSingleton(settings);

    builder.Services.AddMessageBrokerConsumerServicesRabbitMq(
    new RabbitMqConfiguration
    {
        BootstrapServers = settings.BrokerService
    });
    builder.Services.AddSingleton<INewMapPointCommand, NewMapPointCommand>();
    builder.Services.AddSingleton<INewMapCommand, NewMapCommand>();
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

