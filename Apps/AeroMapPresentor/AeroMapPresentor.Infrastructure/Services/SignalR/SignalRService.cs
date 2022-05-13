using AeroMapPresentor.Core.Configurations;
using AeroMapPresentor.Core.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Infrastructure.Services.SignalR;

public class SignalRService : ISignalRService
{
    private readonly ILogger<SignalRService> _logger;
    private readonly IRetryPolicy _retryPolicy;
    private HubConnection _connection;
    private readonly Settings _settings;

    public SignalRService(ILogger<SignalRService> logger, Settings settings,
        IRetryPolicy retryPolicy)
    {
        _logger = logger;
        _settings = settings;
        _retryPolicy = retryPolicy;
        _connection = BuildConnection();
    }

    private HubConnection BuildConnection()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(_settings.WebSocketUrl, HttpTransportType.WebSockets)
            .WithAutomaticReconnect(_retryPolicy)
            .Build();
        return _connection;
    }

    public async Task ConnectAsync()
    {
        try
        {
            _logger.LogInformation("Wpf, SignalR ConnectAsync");
            if (_connection.State != HubConnectionState.Connected)
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
            _logger.LogInformation("Wpf, SignalR DisconnectAsync");
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalRService => Stop connection fail!");
        }
    }

    public void NewMapPoint(Action<string>? callBackAction)
    {
        _connection.On<string>(_settings.NewMapPointMethod, message =>
        {
            callBackAction?.Invoke(message);
        });
    }

    public void NewMap(Action<string>? callBackAction)
    {
        _connection.On<string>(_settings.NewMapMethod, message =>
        {
            callBackAction?.Invoke(message);
        });
    }
}