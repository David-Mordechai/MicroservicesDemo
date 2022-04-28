using System.ComponentModel;
using System.Text.Json;
using AeroMapPresentor.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Infrastructure.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
{
    private const string Api = "http://localhost:5000/api";
    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private byte[]? _imageSource;

    public byte[]? ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            OnPropertyChanged("ImageSource");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public record ResultModel(bool Success, string MapFileAsBase64String, string ErrorMessage = "");

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, 
        IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task SetMissionMapImageSource()
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(_configuration["missionMapApi"]);

            var stringResult = await response.Content.ReadAsStringAsync();
            var resultModel = JsonSerializer.Deserialize<ResultModel>(stringResult,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (resultModel is { Success: true })
            {
                ImageSource = Convert.FromBase64String(resultModel.MapFileAsBase64String);
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