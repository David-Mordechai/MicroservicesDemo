using System.Windows;
using AeroMapPresenter.Mvvm.Wpf.ViewModels;
using AeroMapPresentor.Core.Configurations;
using AeroMapPresentor.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AeroMapPresenter.Mvvm.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
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
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IMapEntitiesProvider, MapEntitiesProvider>();

                    var settings = context.Configuration.GetSection("Settings").Get<Settings>();
                    services.AddAeroMapPresenterInfrastructureServices(settings);
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<MainWindow>();
                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host.Start();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
