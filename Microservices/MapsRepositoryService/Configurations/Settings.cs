namespace MapsRepositoryService.Configurations;

public class Settings
{
    public string BrokerService { get; set; } = "";
    public string NewMissionMapTopic { get; set; } = "";
    public string MapDbService { get; set; } = "";
    public string MapDbRootUser { get; set; } = "";
    public string MapDbRootPassword { get; set; } = "";
}