using Aero.Core.Logger;
using Microsoft.Extensions.Logging;

namespace Aero.Infrastructure.Logger;

internal class AeroLogger<T> : IAeroLogger<T>
{
    private readonly ILogger<T> _logger;

    public AeroLogger(ILogger<T> logger)
    {
        _logger = logger;
    }
    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }
}