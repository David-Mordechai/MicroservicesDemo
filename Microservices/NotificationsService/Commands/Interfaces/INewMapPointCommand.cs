namespace NotificationsService.Commands.Interfaces
{
    public interface INewMapPointCommand
    {
        (bool success, string errorMessage) NewMapPoint(string message);
    }
}