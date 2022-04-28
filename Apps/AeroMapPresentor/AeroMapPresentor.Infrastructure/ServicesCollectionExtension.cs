using AeroMapPresentor.Core.Services;
using AeroMapPresentor.Core.ViewModels;
using AeroMapPresentor.Infrastructure.Services.SignalR;
using AeroMapPresentor.Infrastructure.ViewModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace AeroMapPresentor.Infrastructure;

public static class ServicesCollectionExtension
{
    public static void AddAeroMapPresenterInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IRetryPolicy, RetryPolicy>();
        services.AddSingleton<ISignalRService, SignalRService>();
        services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
    }
}