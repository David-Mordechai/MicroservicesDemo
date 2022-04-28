namespace AeroMapPresentor.Core.Services;

public interface ISignalRService
{
    Task ConnectAsync();
    Task DisconnectAsync();
    void NewMapPoint(Action<string> callBackAction);
    void NewMap(Action<string> callBackAction);
}