using System.Diagnostics.CodeAnalysis;

namespace MapEntitiesService.Core.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record ResultModel(bool Success, string ErrorMessage);