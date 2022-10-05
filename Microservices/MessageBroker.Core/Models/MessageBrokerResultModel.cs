namespace MessageBroker.Core.Models;

public class MessageBrokerResultModel
{
    public string Message { get; init; } = string.Empty;
    public bool Success { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
}