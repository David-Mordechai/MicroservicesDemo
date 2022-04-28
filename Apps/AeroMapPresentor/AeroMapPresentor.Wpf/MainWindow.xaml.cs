using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AeroMapPresentor.Wpf.Services.SignalR;
using AeroMapPresentor.Wpf.ViewModels;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Wpf;

public partial class MainWindow
{
    private readonly ILogger<MainWindow> _logger;
    private readonly ISignalRService _signalRService;
    private readonly Task<Task> _signalRTask;
    private readonly IMainWindowViewModel _mainWindowVm;

    public MainWindow(ILogger<MainWindow> logger, ISignalRService signalRService, IMainWindowViewModel mainWindowView)
    {
        InitializeComponent();
        _logger = logger;
        _signalRService = signalRService;
        _signalRTask = Task.Factory.StartNew(ConfigureSignalR);
        
        _logger.LogInformation("Wpf, MainWindow");

        _mainWindowVm = mainWindowView;
        DataContext = _mainWindowVm;
        _mainWindowVm.SetImageSource();
    }

    private async Task ConfigureSignalR()
    {
        await _signalRService.ConnectAsync();
        _signalRService.NewMapPoint(NewMapPointCommand);
        _signalRService.NewMap(NewMapCommand);
    }

    private void NewMapCommand(string message)
    {
        _logger.LogInformation(message);
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, 
            async () => await _mainWindowVm.SetImageSource());
    }

    private void NewMapPointCommand(string message)
    {
        _logger.LogInformation(message);
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, () =>
        {
            _mainWindowVm.CreateMapEntity(message, CanvasEntities);
        });
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _signalRService.DisconnectAsync();
        _signalRTask.Dispose();
        base.OnClosing(e);
    }
}
