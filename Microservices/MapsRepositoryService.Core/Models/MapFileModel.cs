namespace MapsRepositoryService.Core.Models;

public class MapFileModel
{
    public string? FileName { get; init; } = string.Empty;
    public Stream? File { get; init; }
    public string? FileExtension { get; init; }
}