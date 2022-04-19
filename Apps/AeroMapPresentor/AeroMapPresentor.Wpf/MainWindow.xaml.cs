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
            .WithAutomaticReconnect(new RetryPolicy())
            .Build();

        connection.On<string>("NewMapPoint", message =>
        {
            this.Dispatcher.Invoke(() =>
            {
                _logger.LogInformation(message);
            });
        });

        connection.On<string>("NewMap", message =>
        {
            this.Dispatcher.Invoke(() =>
            {
                _logger.LogInformation(message);
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

    private class RetryPolicy : IRetryPolicy
    {
        ///<summary>
        ///Retry count less then 50: interval 1s;
        ///Retry count less then 250: interval 30s;
        ///Retry count >= 250: interval 1m
        ///</summary>
        ///<param name="retryContext"></param>
        ///<returns></returns>
        public TimeSpan? NextRetryDelay(RetryContext retryContext)
        {
            var count = retryContext.PreviousRetryCount / 50;

            return count switch
            {
                // Retry count <50, interval 1s
                < 50 => new TimeSpan(0, 0, 1),
                // Retry count <250: interval 30s
                < 250 => new TimeSpan(0, 0, 30),
                _ => new TimeSpan(0, 1, 0)
            };
        }
    }
}