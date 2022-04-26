namespace NotificationsService.Commands.Interfaces
{
    public interface INewMapCommand
    {
        (bool success, string errorMessage) NewMap(string message);
    }
}