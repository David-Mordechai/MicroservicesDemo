using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Wpf.Services.SignalR;

public class SignalRService : ISignalRService
{
    private readonly ILogger<SignalRService> _logger;
    private readonly IRetryPolicy _retryPolicy;
    private HubConnection _connection;

    public SignalRService(ILogger<SignalRService> logger, IRetryPolicy retryPolicy)
    {
        _logger = logger;
        _retryPolicy = retryPolicy;
        _connection = BuildConnection();
    }

    private HubConnection BuildConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/ws", HttpTransportType.WebSockets)
            .WithAutomaticReconnect(_retryPolicy)
            .Build();
        return _connection;
    }

    public async Task ConnectAsync()
    {
        try
        {
            if(_connection.State != HubConnectionState.Connected)
                await _connection.StartAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalRService => Start connection fail!");
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalRService => Start connection fail!");
        }
    }

    public void NewMapPoint(Action<string>? callBackAction)
    {
        _connection.On<string>("NewMapPoint", message =>
        {
            callBackAction?.Invoke(message);
        });
    }

    public void NewMap(Action<string>? callBackAction)
    {
        _connection.On<string>("NewMap", message =>
        {
            callBackAction?.Invoke(message);
        });
    }
}