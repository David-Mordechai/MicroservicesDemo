using Microsoft.AspNetCore.SignalR;
using NotificationsService.EventHandlers.Interfaces;
using NotificationsService.Hubs;

namespace NotificationsService.EventHandlers
{
    public class NewMapPointEventHandler : INewMapPointEventHandler
    {
        private readonly ILogger<NewMapPointEventHandler> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NewMapPointEventHandler(
            ILogger<NewMapPointEventHandler> logger,
            IHubContext<NotificationsHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public (bool success, string errorMessage) NewMapPoint(string message)
        {
            _logger.LogInformation("NotificationsService, New MapPoint {mapEntity}", message);
            _hubContext.Clients.All.SendAsync("NewMapPoint", message);
            return (true, string.Empty);
        }
    }
}
