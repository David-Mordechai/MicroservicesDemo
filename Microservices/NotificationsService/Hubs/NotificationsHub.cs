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
    

    private (bool success, string errorMessage) ConsumeMessageHandler(string message)
    {
        _logger.LogInformation("NotificationsService, New AeroMapEntity {mapEntity}", message);
        _hubContext.Clients.All.SendAsync("NewMapPoint", message);
        return (true, string.Empty);
    }

    public override Task OnConnectedAsync()
    {
        _subscriber.Subscribe("NewMapEntity", ConsumeMessageHandler, CancellationToken.None);
        return base.OnConnectedAsync();
    }
}