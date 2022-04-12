namespace MapsRepositoryService.Core.Models;

public class MapFileModel
{
    public string MapName { get; set; } = string.Empty;
    public MemoryStream? MapFile { get; set; }
}