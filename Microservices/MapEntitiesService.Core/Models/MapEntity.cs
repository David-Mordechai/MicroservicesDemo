using System.Diagnostics.CodeAnalysis;

namespace MapEntitiesService.Core.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record MapEntity(string Title, double XPosition, double YPosition, double MapWidth, double MapHeight);