namespace MapsRepositoryService.Infrastructure.MinIo.Configuration;

public class MinIoConfiguration
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string RootUser { get; set; } = string.Empty;
    public string RootPassword { get; set; } = string.Empty;
}