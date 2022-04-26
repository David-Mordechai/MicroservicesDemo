using System.Windows;
using AeroMapPresentor.Wpf.Services.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AeroMapPresentor.Wpf;

public partial class App
{
    
    private readonly IHost _host;
    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .UseEnvironment("Development")
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
            }).
            UseSerilog((_, configuration) => configuration.WriteTo.Seq("http://localhost:5000/log"))
            .ConfigureServices(services =>
            {
                services.AddHttpClient();
                services.AddSingleton<IRetryPolicy, RetryPolicy>();
                services.AddSingleton<ISignalRService, SignalRService>();
                services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
                services.AddSingleton<MainWindow>();
            }).Build();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        _host.Start();
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}