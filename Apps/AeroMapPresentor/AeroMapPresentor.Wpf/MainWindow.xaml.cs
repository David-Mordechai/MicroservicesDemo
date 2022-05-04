using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AeroMapPresentor.Core.Models;
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
    private readonly IMainWindowViewModel _mainWindowViewModelModel;
    private readonly IEllipseEntityUiCreator _ellipseEntityUiCreator;
    private double _windowHeight;
    private double _windowWidth;
    private readonly List<(UIElement stackPanel, MapEntity mapEntity)> _mapPointsUiElementsList = new();

    public MainWindow(ILogger<MainWindow> logger, ISignalRService signalRService, 
        IMainWindowViewModel mainWindowViewModel, IEllipseEntityUiCreator ellipseEntityUiCreator)
    {
        _logger = logger;
        _signalRService = signalRService;
        _signalRTask = Task.Factory.StartNew(ConfigureSignalR);
        _mainWindowViewModelModel = mainWindowViewModel;
        _ellipseEntityUiCreator = ellipseEntityUiCreator;

        _mainWindowViewModelModel.SetMissionMapImageSource();
        DataContext = _mainWindowViewModelModel;
        SizeChanged += OnWindowSizeChanged;
        
        InitializeComponent();
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
            await _mainWindowViewModelModel.SetMissionMapImageSource();
            CanvasEntities.Children.Clear();
            _mapPointsUiElementsList.Clear();
        });
    }
    
    private void NewMapPointCommand(string newMapEntity)
    {
        _logger.LogInformation("AeroMapPresentor.Wpf => New Map Point: {newMapEntity}", newMapEntity);
        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, () =>
        {
            var mapEntity = JsonSerializer.Deserialize<MapEntity>(newMapEntity,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            
            if(mapEntity is null) return;

            var stackPanel = _ellipseEntityUiCreator.Create(mapEntity);
            CanvasEntities.Children.Add(stackPanel);

            _mapPointsUiElementsList.Add((stackPanel, mapEntity));
            SetPosition(mapEntity, stackPanel);
        });
    }

    private void SetPosition(MapEntity mapEntity, UIElement stackPanel)
    {
        var top = _windowHeight / mapEntity.MapHeight * mapEntity.XPosition;
        var left = _windowWidth / mapEntity.MapWidth * mapEntity.YPosition;
        Canvas.SetTop(stackPanel, top);
        Canvas.SetLeft(stackPanel, left);
    }
    
    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
        _windowHeight = e.NewSize.Height;
        _windowWidth = e.NewSize.Width;

        Task.Run(() =>
        {
            foreach (var (stackPanel, mapEntity) in _mapPointsUiElementsList)
            {
                Dispatcher.Invoke(DispatcherPriority.Render,() =>
                {
                    SetPosition(mapEntity, stackPanel);
                });
            }
        });   
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _signalRService.DisconnectAsync();
        _signalRTask.Dispose();
        base.OnClosing(e);
    }
}