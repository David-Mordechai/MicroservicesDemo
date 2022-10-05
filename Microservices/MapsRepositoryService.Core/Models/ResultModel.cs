using System.Diagnostics.CodeAnalysis;

namespace MapsRepositoryService.Core.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record ResultModel(bool Success, string ErrorMessage = "");