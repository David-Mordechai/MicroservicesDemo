using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.Logging;

namespace AeroMapPresentor.Wpf.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
{
    private const string Api = "http://localhost:5000/api";

    private readonly ILogger<MainWindowViewModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public record ResultModel(bool Success, string MapFileAsBase64String, string ErrorMessage = "");
    public record MapEntity(string Title, double XPosition, double YPosition);

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    private ImageSource? _imageSource;

    public ImageSource? ImageSource
    {
        get => _imageSource;
        set
        {
            _imageSource = value;
            OnPropertyChanged("ImageSource");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public async Task SetImageSource()
    {
        try
        {
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{Api}/mission");

            var stringResult = await response.Content.ReadAsStringAsync();
            var resultModel = JsonSerializer.Deserialize<ResultModel>(stringResult,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (resultModel is { Success: true })
            {
                var bytes = Convert.FromBase64String(resultModel.MapFileAsBase64String);
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(bytes);
                image.EndInit();
                image.Freeze();
                ImageSource = image;
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Set Mission Map failed");
        }
    }

    public void CreateMapEntity(string message, Canvas canvasEntities)
    {
        try
        {
            var mapEntity = JsonSerializer.Deserialize<MapEntity>(message,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            StackPanel stackPanel = BuildMapEntity(mapEntity!);
            canvasEntities.Children.Add(stackPanel);
            Canvas.SetTop(stackPanel, mapEntity!.XPosition);
            Canvas.SetLeft(stackPanel, mapEntity.YPosition);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static StackPanel BuildMapEntity(MapEntity mapEntity)
    {
        var ellipse = new Ellipse
        {
            Fill = Brushes.Purple,
            Stroke = Brushes.MediumPurple,
            Width = 20,
            Height = 20,
            StrokeThickness = 2
        };

        var textBlock = new TextBlock
        {
            Foreground = Brushes.Purple,
            FontSize = 12,
            Text = mapEntity!.Title
        };

        var stackPanel = new StackPanel();
        stackPanel.Children.Add(ellipse);
        stackPanel.Children.Add(textBlock);
        return stackPanel;
    }

    protected void OnPropertyChanged(string name)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}