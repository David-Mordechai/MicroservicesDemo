namespace NotificationsService.EventHandlers.Interfaces;

public interface INewMapEventHandler
{
    (bool success, string errorMessage) NewMap(string message);
}