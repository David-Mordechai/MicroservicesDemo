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
using System.Windows.Threading;
using AeroMapPresentor.Wpf.Services.SignalR;
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
        _mainWindowVm.SetImageSource();
    }

    private void NewMapPointCommand(string message)
    {
        _logger.LogInformation(message);
        _mainWindowVm.CreateMapEntity(message, CanvasEntities);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _signalRService.DisconnectAsync();
        _signalRTask.Dispose();
        base.OnClosing(e);
    }
}

public interface IMainWindowViewModel
{
    Task SetImageSource();
    void CreateMapEntity(string message, Canvas canvasEntities);
}

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
        var client = _httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync($"{Api}/mission");

            var stringResult = await response.Content.ReadAsStringAsync();
            var resultModel = JsonSerializer.Deserialize<ResultModel>(stringResult,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (resultModel is {Success: true})
            {
                SetImageSource(Convert.FromBase64String(resultModel.MapFileAsBase64String));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Set Mission Map failed");
        }
    }

    public void CreateMapEntity(string message, Canvas canvasEntities)
    {
        var mapEntity = JsonSerializer.Deserialize<MapEntity>(message, 
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
        //try
        //{
        //    Dispatcher.CurrentDispatcher.Invoke(() =>
        //    {
        //        var ellipse = new Ellipse
        //        {
        //            Fill = Brushes.Purple,
        //            Stroke = Brushes.MediumPurple,
        //            Width = 10,
        //            Height = 10,
        //            StrokeThickness = 2
        //        };

        //        var textBox = new TextBox
        //        {
        //            Foreground = Brushes.White,
        //            FontSize = 10,
        //            Text = mapEntity!.Title
        //        };

        //        var stackPanel = new StackPanel();
        //        stackPanel.Children.Add(ellipse);
        //        stackPanel.Children.Add(textBox);

        //        canvasEntities.Children.Add(stackPanel);
        //        Canvas.SetTop(stackPanel, mapEntity.XPosition);
        //        Canvas.SetLeft(stackPanel, mapEntity.YPosition);
        //    }, DispatcherPriority.Normal);
            
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw;
        //}
    }

    private void SetImageSource(byte[] imageData)
    {
        var image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = new MemoryStream(imageData);
        image.EndInit();
        image.Freeze();
        Dispatcher.CurrentDispatcher.Invoke(() =>  ImageSource = image);
    }

    protected void OnPropertyChanged(string name)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}