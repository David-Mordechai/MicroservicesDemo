using AeroMapPresentor.Core.Configurations;
using AeroMapPresentor.Core.Services;
using AeroMapPresentor.Infrastructure;
using Serilog;

var app = Host.CreateDefaultBuilder()
    .UseSerilog((context, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(context.Configuration))
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient();
        var settings = context.Configuration.GetSection("Settings").Get<Settings>();
        services.AddAeroMapPresenterInfrastructureServices(settings);
        services.AddHostedService<Worker>();
        services.AddSignalR();
    }).Build();

app.Run();

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISignalRService _signalRService;

    public Worker(ILogger<Worker> logger, ISignalRService signalRService)
    {
        _logger = logger;
        _signalRService = signalRService;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        ConfigureSignalR();
        return Task.CompletedTask;
    }
    
    private async void ConfigureSignalR()
    {
        _signalRService.NewMapPoint(NewMapPointCommand);
        _signalRService.NewMap(NewMissionMapCommand);
        await _signalRService.ConnectAsync();
    }

    private void NewMissionMapCommand(string newMissionMapName)
    {
        _logger.LogInformation("AeroMapPresenter.Console => New Mission Map: {NewMissionMapName}", newMissionMapName);
    }
    
    private void NewMapPointCommand(string newMapEntity)
    {
        _logger.LogInformation("AeroMapPresenter.Console => New Map Point: {NewMapEntity}", newMapEntity);
    }
}