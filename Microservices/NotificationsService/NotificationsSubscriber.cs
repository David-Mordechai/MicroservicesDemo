using MessageBroker.Core;
using NotificationsService.EventHandlers.Interfaces;

namespace NotificationsService;

public class NotificationsSubscriber
{
    private readonly ISubscriber _subscriber;
    private readonly INewMapPointEventHandler _newMapPointEventHandler;
    private readonly INewMapEventHandler _newMapEventHandler;

    public NotificationsSubscriber(
        ISubscriber subscriber,
        INewMapPointEventHandler newMapPointEventHandler,
        INewMapEventHandler newMapEventHandler)
    {
        _subscriber = subscriber;
        _newMapPointEventHandler = newMapPointEventHandler;
        _newMapEventHandler = newMapEventHandler;
    }

    public void NewMapPointSubscribe()
    {
        _subscriber.Subscribe("NewMapEntity", _newMapPointEventHandler.NewMapPoint, CancellationToken.None);
    }

    public void NewMapSubscribe()
    {
        _subscriber.Subscribe("NewMapUploaded", _newMapEventHandler.NewMap, CancellationToken.None);
    }
}