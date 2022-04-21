using System;
using System.Threading.Tasks;

namespace AeroMapPresentor.Wpf.Services.SignalR;

public interface ISignalRService
{
    Task ConnectAsync();
    Task DisconnectAsync();
    void NewMapPoint(Action<string> callBackAction);
    void NewMap(Action<string> callBackAction);
}