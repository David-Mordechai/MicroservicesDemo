using System.Diagnostics.CodeAnalysis;

namespace MapsRepositoryService.Core.Models;

public class MapListItemModel
{
    public string MapName { get; init; } = string.Empty;
    
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] 
    public bool IsMissionMap { get; set; }
}