using System.Diagnostics.CodeAnalysis;

namespace MapsRepositoryService.Core.Models;

[SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Global")]
public record UploadMapResultModel(bool Success, string ControlName = "", string ErrorMessage = "");