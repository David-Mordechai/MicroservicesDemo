namespace Aero.Core.Logger;

public interface IAeroLogger<T>
{
    void LogInformation(string message, params object[] args);
    void LogError(string message, params object[] args);
}