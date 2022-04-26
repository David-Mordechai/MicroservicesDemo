using MessageBroker.Core;
using NotificationsService.Commands.Interfaces;

namespace NotificationsService;

public class NotificationsSubscriber
{
    private readonly ISubscriber _subscriber;
    private readonly INewMapPointCommand _newMapPointCommand;
    private readonly INewMapCommand _newMapCommand;

    public NotificationsSubscriber(
        ISubscriber subscriber,
        INewMapPointCommand newMapPointCommand,
        INewMapCommand newMapCommand)
    {
        _subscriber = subscriber;
        _newMapPointCommand = newMapPointCommand;
        _newMapCommand = newMapCommand;
    }

    public void Subscribe()
    {
        NewMapPointSubscribe();
        NewMapSubscribe();
    }

    private void NewMapPointSubscribe()
    {
        _subscriber.Subscribe("NewMapEntity", _newMapPointCommand.NewMapPoint, CancellationToken.None);
    }

    private void NewMapSubscribe()
    {
        _subscriber.Subscribe("NewMissionMap", _newMapCommand.NewMap, CancellationToken.None);
    }
}