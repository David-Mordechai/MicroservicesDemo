namespace MapsRepositoryService.Core.Models;

public class MapFileModel
{
    public string FileName { get; set; } = string.Empty;
    public Stream? MapFile { get; set; }
}