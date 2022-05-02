using Microsoft.AspNetCore.SignalR;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Configurations;
using NotificationsService.Hubs;

namespace NotificationsService.Commands;

public class NewMapCommand : INewMapCommand
{
    private readonly ILogger<NewMapCommand> _logger;
    private readonly IHubContext<NotificationsHub> _hubContext;
    private readonly IConfiguration _configuration;

    public NewMapCommand(
        ILogger<NewMapCommand> logger,
        IHubContext<NotificationsHub> hubContext,
        IConfiguration configuration)
    {
        _logger = logger;
        _hubContext = hubContext;
        _configuration = configuration;
    }

    public (bool success, string errorMessage) NewMap(string message)
    {
        _logger.LogInformation("NotificationsService, New Map {mapFileName}", message);
        var clientMethods = _configuration.GetSection("Settings").Get<Settings>();
        _hubContext.Clients.All.SendAsync(clientMethods.NewMapClientMethod, message);
        return (true, string.Empty);
    }
}