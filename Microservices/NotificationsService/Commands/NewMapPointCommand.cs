using Microsoft.AspNetCore.SignalR;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;
using NotificationsService.Hubs;

namespace NotificationsService.Commands;

public class NewMapPointCommand : INewMapPointCommand
{
    private readonly ILogger<NewMapPointCommand> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly Settings _settings;

    public NewMapPointCommand(
        ILogger<NewMapPointCommand> logger,
        IHubContext<NotificationsHub> hubContext,
        Settings settings)
    {
        _logger = logger;
        _hubContext = hubContext;
        _settings = settings;
    }

    public (bool success, string errorMessage) NewMapPoint(string message)
    {
        _logger.LogInformation("NotificationsService, New MapPoint {MapEntity}", message);
        _hubContext.Clients.All.SendAsync(_settings.NewMapPointClientMethod, message);
        return (true, string.Empty);
    }
}