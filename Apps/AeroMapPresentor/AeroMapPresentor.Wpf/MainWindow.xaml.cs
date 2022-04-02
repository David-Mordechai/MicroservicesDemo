using System.Threading;
using Aero.Core.Logger;
using Aero.Core.MessageBroker;

namespace AeroMapPresentor.Wpf;

public partial class MainWindow
{
    private readonly IAeroLogger<MainWindow> _logger;

    public MainWindow(IAeroLogger<MainWindow> logger, ISubscriber subscriber)
    {

        _logger = logger;
        subscriber.Subscribe("NewMapEntity", ConsumeMessageHandler, CancellationToken.None);
        InitializeComponent();
    }

    private (bool success, string errorMessage) ConsumeMessageHandler(string message)
    {
        _logger.LogInformation("Wpf, New AeroMapEntity {mapEntity}", message);
        return (true,string.Empty);
    }
}