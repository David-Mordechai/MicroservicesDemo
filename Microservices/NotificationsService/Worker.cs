using MessageBroker.Core;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;

namespace NotificationsService;

public class Worker : BackgroundService
{
    private readonly ISubscriber _subscriber;
    private readonly INewMapPointCommand _newMapPointCommand;
    private readonly INewMapCommand _newMapCommand;
    private readonly Settings _settings;

    public Worker(
        ISubscriber subscriber,
        INewMapPointCommand newMapPointCommand,
        INewMapCommand newMapCommand,
        Settings settings)
    {
        _subscriber = subscriber;
        _newMapPointCommand = newMapPointCommand;
        _newMapCommand = newMapCommand;
        _settings = settings;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _subscriber.Subscribe(_settings.NewMapPointTopic, _newMapPointCommand.NewMapPoint, stoppingToken);
        _subscriber.Subscribe(_settings.NewMapTopic, _newMapCommand.NewMap, stoppingToken);
        return Task.CompletedTask;
    }
}