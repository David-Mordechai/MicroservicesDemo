using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Wpf;

public partial class MainWindow
{
    private readonly ILogger<MainWindow> _logger;
    HubConnection connection;

    public MainWindow(ILogger<MainWindow> logger)
    {

        _logger = logger;
        InitializeComponent();
        _logger.LogInformation("Wpf, MainWindow");

        Task.Factory.StartNew(async () => await SignalRService());
    }

    private async Task SignalRService()
    {
        connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/ws",HttpTransportType.WebSockets)
            //.WithAutomaticReconnect()
            .Build();

        connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };

        connection.On<string>("NewMapPoint", (message) =>
        {
            this.Dispatcher.Invoke(() =>
            {
                var newMessage = $"{message}";
                _logger.LogInformation(newMessage);
            });
        });

        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR => onNewMapPoint error");
        }
    }
}