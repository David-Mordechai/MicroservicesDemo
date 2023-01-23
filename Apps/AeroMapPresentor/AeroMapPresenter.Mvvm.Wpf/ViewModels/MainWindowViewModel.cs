using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AeroMapPresentor.Core.Configurations;
using AeroMapPresentor.Core.Models;
using AeroMapPresentor.Core.Services;
using Microsoft.Extensions.Logging;

namespace AeroMapPresenter.Mvvm.Wpf.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private byte[]? _imageSource;
    private readonly Settings _settings;
    private readonly ISignalRService _signalRService;

    public ObservableCollection<MapEntityViewModel> Entities { get; set; }

    public byte[]? ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            SetField(ref _imageSource, value);
        }
    }

    public record MapResultModel(string ImageMetaData, string ImageBase64);
    public record ResultModel(bool Success, MapResultModel MapFileAsBase64String, string ErrorMessage = "");

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger,
        IHttpClientFactory httpClientFactory, Settings settings, ISignalRService signalRService)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _settings = settings;
        _signalRService = signalRService;

        Entities = new ObservableCollection<MapEntityViewModel>();
        _signalRService.NewMap(OnNewMapCallback);
        _signalRService.NewMapPoint(OnNewMapPointCallback);

        _signalRService.ConnectAsync();

        SetMissionMapImageSource();

    }

    private async void ConfigureSignalR()
    {
        _signalRService.NewMapPoint(NewMapPointCommand);
        _signalRService.NewMap(NewMissionMapCommand);
        await _signalRService.ConnectAsync();
    }

    private void OnNewMapPointCallback(string newMapEntity)
    {
        _logger.LogInformation("AeroMapPresentor.Wpf => New Map Point: {newMapEntity}", newMapEntity);
        var mapEntity = JsonSerializer.Deserialize<MapEntity>(newMapEntity,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        if (mapEntity is null) return;

        var mapEntityVm = new MapEntityViewModel(mapEntity);

        //var stackPanel = _ellipseEntityUiCreator.Create(mapEntity);
        //CanvasEntities.Children.Add(stackPanel);

        //_mapPointsUiElementsList.Add((stackPanel, mapEntity));
        //SetPosition(mapEntity, stackPanel);

        Entities.Add(mapEntityVm);
    }

    private void OnNewMapCallback(string obj)
    {
        SetMissionMapImageSource();
    }

    private async void SetMissionMapImageSource()
    {
        ImageSource = await GetMissionMapImageSource();
    }

    public async Task<byte[]?> GetMissionMapImageSource()
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var resultModel = await client.GetFromJsonAsync<ResultModel>(_settings.MissionMapApi);

            if (resultModel is { Success: true })
            {
                return Convert.FromBase64String(resultModel.MapFileAsBase64String.ImageBase64);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Set Mission Map failed");
        }

        return default;
    }

}