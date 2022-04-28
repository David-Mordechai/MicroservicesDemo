using System.Windows;
using AeroMapPresentor.Infrastructure;
using AeroMapPresentor.Wpf.UiComponents;
using AeroMapPresentor.Wpf.UiComponents.Interfaces;
using Microsoft.Extensions.Configuration;
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
            .ConfigureAppConfiguration( builder =>
            {
                builder.Sources.Clear();
                // appsettings.json Important!!!
                // change in file properties => "Copy To Output Directory" to "Copy Always"
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .UseEnvironment("Development")
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
            })
            .UseSerilog((context, loggerConfiguration) =>
                loggerConfiguration.ReadFrom.Configuration(context.Configuration))
            .ConfigureServices(services =>
            {
                services.AddHttpClient();
                services.AddTransient<IEllipseEntityUiCreator, EllipseEntityUiCreator>();
                services.AddAeroMapPresenterInfrastructureServices();
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