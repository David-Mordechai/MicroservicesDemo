using MessageBroker.Core;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;

namespace NotificationsService;

public class NotificationsSubscriber
{
    private readonly ISubscriber _subscriber;
    private readonly INewMapPointCommand _newMapPointCommand;
    private readonly INewMapCommand _newMapCommand;
    private readonly Settings _settings;

    public NotificationsSubscriber(
        ISubscriber subscriber,
        INewMapPointCommand newMapPointCommand,
        INewMapCommand newMapCommand,
        IConfiguration configuration)
    {
        _subscriber = subscriber;
        _newMapPointCommand = newMapPointCommand;
        _newMapCommand = newMapCommand;
        _settings = configuration.GetSection("Settings").Get<Settings>();
    }

    public void Subscribe()
    {
        NewMapPointSubscribe();
        NewMapSubscribe();
    }

    private void NewMapPointSubscribe()
    {
        _subscriber.Subscribe(_settings.NewMapPointTopic, _newMapPointCommand.NewMapPoint, CancellationToken.None);
    }

    private void NewMapSubscribe()
    {
        _subscriber.Subscribe(_settings.NewMapTopic, _newMapCommand.NewMap, CancellationToken.None);
    }
}