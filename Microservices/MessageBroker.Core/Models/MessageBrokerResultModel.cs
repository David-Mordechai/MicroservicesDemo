namespace MessageBroker.Core.Models;

public class MessageBrokerResultModel
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}