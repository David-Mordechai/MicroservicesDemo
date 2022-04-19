using MessageBroker.Core;
using Microsoft.AspNetCore.SignalR;

namespace NotificationsService.Hubs;

public class NotificationsHub : Hub
{
    private readonly ILogger<NotificationsHub> _logger;
    private readonly ISubscriber _subscriber;
    private readonly IHubContext<NotificationsHub> _hubContext;

    public NotificationsHub(
        ILogger<NotificationsHub> logger,
        ISubscriber subscriber,
        IHubContext<NotificationsHub> hubContext)
    {
        _logger = logger;
        _subscriber = subscriber;
        _hubContext = hubContext;
    }

    public override Task OnConnectedAsync()
    {
        _subscriber.Subscribe("NewMapEntity", NewMapPointHandler, CancellationToken.None);
        _subscriber.Subscribe("NewMapUploaded", NewMapHandler, CancellationToken.None);
        return base.OnConnectedAsync();
    }

    private (bool success, string errorMessage) NewMapPointHandler(string message)
    {
        _logger.LogInformation("NotificationsService, New MapPoint {mapEntity}", message);
        _hubContext.Clients.All.SendAsync("NewMapPoint", message);
        return (true, string.Empty);
    }

    private (bool success, string errorMessage) NewMapHandler(string message)
    {
        _logger.LogInformation("NotificationsService, New Map {mapFileName}", message);
        _hubContext.Clients.All.SendAsync("NewMap", message);
        return (true, string.Empty);
    }
}