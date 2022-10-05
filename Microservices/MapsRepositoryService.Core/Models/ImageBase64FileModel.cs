using System.Diagnostics.CodeAnalysis;

namespace MapsRepositoryService.Core.Models;

public class ImageBase64FileModel
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] 
    public string ImageMetaData { get; init; } = string.Empty;

    
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")] 
    public string ImageBase64 { get; init; } = string.Empty;
}