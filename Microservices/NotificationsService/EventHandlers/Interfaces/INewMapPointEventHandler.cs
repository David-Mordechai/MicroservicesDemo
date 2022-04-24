namespace NotificationsService.EventHandlers.Interfaces
{
    public interface INewMapPointEventHandler
    {
        (bool success, string errorMessage) NewMapPoint(string message);
    }
}