namespace Aero.Core.MessageBroker.Models;

public class MessageResultModel
{
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}