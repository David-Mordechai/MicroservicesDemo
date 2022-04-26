using Microsoft.AspNetCore.SignalR;
using NotificationsService.Commands.Interfaces;
using NotificationsService.Hubs;

namespace NotificationsService.Commands
{
    public class NewMapPointCommand : INewMapPointCommand
    {
        private readonly ILogger<NewMapPointCommand> _logger;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NewMapPointCommand(
            ILogger<NewMapPointCommand> logger,
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
