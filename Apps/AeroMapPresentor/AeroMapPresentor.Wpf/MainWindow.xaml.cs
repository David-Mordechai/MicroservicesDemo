using System.ComponentModel;
using System.Threading.Tasks;
using AeroMapPresentor.Wpf.Services.SignalR;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Wpf;

public partial class MainWindow
{
    private readonly ILogger<MainWindow> _logger;
    private readonly ISignalRService _signalRService;
    private readonly Task<Task> _signalRTask;

    public MainWindow(ILogger<MainWindow> logger, ISignalRService signalRService)
    {
        _logger = logger;
        _signalRService = signalRService;
        _logger.LogInformation("Wpf, MainWindow");
        _signalRTask = Task.Factory.StartNew(ConfigureSignalR);
        InitializeComponent();
    }

    private async Task ConfigureSignalR()
    {
        await _signalRService.ConnectAsync();
        _signalRService.NewMapPoint(NewMapPointEvent);
        _signalRService.NewMap(NewMapEvent);
    }

    private void NewMapEvent(string message)
    {
        _logger.LogInformation(message);
    }

    private void NewMapPointEvent(string message)
    {
        _logger.LogInformation(message);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _signalRService.DisconnectAsync();
        _signalRTask.Dispose();
        base.OnClosing(e);
    }
}