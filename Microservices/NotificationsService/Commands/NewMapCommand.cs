using Microsoft.AspNetCore.SignalR;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;
using NotificationsService.Hubs;

namespace NotificationsService.Commands;

public class NewMapCommand : INewMapCommand
{
    private readonly ILogger<NewMapCommand> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly Settings _settings;

    public NewMapCommand(
        ILogger<NewMapCommand> logger,
        IHubContext<NotificationsHub> hubContext,
        Settings settings)
    {
        _logger = logger;
        _hubContext = hubContext;
        _settings = settings;
    }

    public (bool success, string errorMessage) NewMap(string message)
    {
        _logger.LogInformation("NotificationsService, New Map {MapFileName}", message);
        _hubContext.Clients.All.SendAsync(_settings.NewMapClientMethod, message);
        return (true, string.Empty);
    }
}