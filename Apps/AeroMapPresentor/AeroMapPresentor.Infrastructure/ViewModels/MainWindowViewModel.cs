using System.ComponentModel;
using System.Net.Http.Json;
using AeroMapPresentor.Core.Configurations;
using AeroMapPresentor.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Infrastructure.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
{
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private byte[]? _imageSource;
    private readonly Settings _settings;

    public byte[]? ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            OnPropertyChanged(nameof(ImageSource));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public record MapResultModel(string ImageMetaData, string ImageBase64);
    public record ResultModel(bool Success, MapResultModel MapFileAsBase64String, string ErrorMessage = "");

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, 
        IHttpClientFactory httpClientFactory, Settings settings)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _settings = settings;
    }

    public async Task SetMissionMapImageSource()
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var resultModel = await client.GetFromJsonAsync<ResultModel>(_settings.MissionMapApi);

            if (resultModel is { Success: true })
            {
                ImageSource = Convert.FromBase64String(resultModel.MapFileAsBase64String.ImageBase64);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Set Mission Map failed");
        }
    }
    
    protected void OnPropertyChanged(string name)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}