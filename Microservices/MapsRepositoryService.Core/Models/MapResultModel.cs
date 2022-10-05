using System.Diagnostics.CodeAnalysis;

namespace MapsRepositoryService.Core.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record MapResultModel(bool Success, ImageBase64FileModel? MapFileAsBase64String, string ErrorMessage = "");