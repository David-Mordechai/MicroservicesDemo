using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AeroMapPresentor.Core.Services;
using AeroMapPresentor.Core.ViewModels;
using AeroMapPresentor.Wpf.UiComponents.Interfaces;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Wpf;

public partial class MainWindow
{
    private readonly ILogger<MainWindow> _logger;
    private readonly ISignalRService _signalRService;
    private readonly Task<Task> _signalRTask;
    private readonly IMainWindowViewModel _mainWindowViewModel;
    private readonly IEllipseEntityUiCreator _ellipseEntityUiCreator;

    public record MapEntity(string Title, double XPosition, double YPosition);

    public MainWindow(ILogger<MainWindow> logger, ISignalRService signalRService, 
        IMainWindowViewModel mainWindowView, IEllipseEntityUiCreator ellipseEntityUiCreator)
    {
        InitializeComponent();
        _logger = logger;
        _logger.LogInformation("Wpf, MainWindow");
        
        _signalRService = signalRService;
        _signalRTask = Task.Factory.StartNew(ConfigureSignalR);

        _mainWindowViewModel = mainWindowView;
        _ellipseEntityUiCreator = ellipseEntityUiCreator;
        _mainWindowViewModel.SetMissionMapImageSource();
        DataContext = _mainWindowViewModel;
    }

    private async Task ConfigureSignalR()
    {
        await _signalRService.ConnectAsync();
        _signalRService.NewMapPoint(NewMapPointCommand);
        _signalRService.NewMap(NewMissionMapCommand);
    }

    private void NewMissionMapCommand(string newMissionMapName)
    {
        _logger.LogInformation("AeroMapPresentor.Wpf => New Mission Map: {newMissionMapName}", newMissionMapName);
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
        async () =>
        {
            await _mainWindowViewModel.SetMissionMapImageSource();
            CanvasEntities.Children.Clear();
        });
    }

    private void NewMapPointCommand(string newMapEntity)
    {
        _logger.LogInformation("AeroMapPresentor.Wpf => New Mission Map: {newMapEntity}", newMapEntity);
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, () =>
        {
            var mapEntity = JsonSerializer.Deserialize<MapEntity>(newMapEntity,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            
            if(mapEntity is null) return;
            
            var stackPanel = _ellipseEntityUiCreator.Create(mapEntity);
            CanvasEntities.Children.Add(stackPanel);
            Canvas.SetTop(stackPanel, mapEntity.XPosition);
            Canvas.SetLeft(stackPanel, mapEntity.YPosition);
        });
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _signalRService.DisconnectAsync();
        _signalRTask.Dispose();
        base.OnClosing(e);
    }
}