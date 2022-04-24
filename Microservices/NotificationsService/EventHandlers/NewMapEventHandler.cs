using Microsoft.AspNetCore.SignalR;
using NotificationsService.EventHandlers.Interfaces;
using NotificationsService.Hubs;

namespace NotificationsService.EventHandlers
{
    public class NewMapEventHandler : INewMapEventHandler
    {
        private readonly ILogger<NewMapEventHandler> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NewMapEventHandler(
            ILogger<NewMapEventHandler> logger,
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
