namespace MapsRepositoryService.Infrastructure.MinIo.Configuration;

public class MinIoConfiguration
{
    public string BootstrapServers { get; init; } = string.Empty;
    public string RootUser { get; init; } = string.Empty;
    public string RootPassword { get; init; } = string.Empty;
}