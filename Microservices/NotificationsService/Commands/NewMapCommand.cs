using Microsoft.AspNetCore.SignalR;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Hubs;

namespace NotificationsService.Commands
{
    public class NewMapCommand : INewMapCommand
    {
        private readonly ILogger<NewMapCommand> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NewMapCommand(
            ILogger<NewMapCommand> logger,
            IHubContext<NotificationsHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public (bool success, string errorMessage) NewMap(string message)
        {
            _logger.LogInformation("NotificationsService, New Map {mapFileName}", message);
            _hubContext.Clients.All.SendAsync("NewMap", message);
            return (true, string.Empty);
        }
    }
}
