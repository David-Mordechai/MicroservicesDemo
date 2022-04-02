using System.Windows;
using Aero.Infrastructure;
using Aero.Infrastructure.MessageBroker.RabbitMq.Builder.Configuration;
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
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
            }).
            UseSerilog((_, configuration) => configuration.WriteTo.Seq("http://localhost:5341"))
            .ConfigureServices(services =>
            {
                services.AddAeroInfrastructure();
                services.AddMessageBrokerConsumerServicesRabbitMq(new RabbitMqConfiguration
                {
                    BootstrapServers = "localhost"
                });
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