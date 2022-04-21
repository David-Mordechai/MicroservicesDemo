using Microsoft.AspNetCore.SignalR;

namespace NotificationsService.Hubs;

public class NotificationsHub : Hub
{
    private readonly ILogger<NotificationsHub> _logger;

    public NotificationsHub(
        ILogger<NotificationsHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("SignalR: Connected!");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("SignalR: Disconnected!");
        return base.OnDisconnectedAsync(exception);
    }
}