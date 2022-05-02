using Microsoft.AspNetCore.SignalR;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;
using NotificationsService.Hubs;

namespace NotificationsService.Commands;

public class NewMapPointCommand : INewMapPointCommand
{
    private readonly ILogger<NewMapPointCommand> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly IConfiguration _configuration;

    public NewMapPointCommand(
        ILogger<NewMapPointCommand> logger,
        IHubContext<NotificationsHub> hubContext,
        IConfiguration configuration)
    {
        _logger = logger;
        _hubContext = hubContext;
        _configuration = configuration;
    }

    public (bool success, string errorMessage) NewMapPoint(string message)
    {
        _logger.LogInformation("NotificationsService, New MapPoint {mapEntity}", message);
        var clientMethods = _configuration.GetSection("Settings").Get<Settings>();
        _hubContext.Clients.All.SendAsync(clientMethods.NewMapPointClientMethod, message);
        return (true, string.Empty);
    }
}