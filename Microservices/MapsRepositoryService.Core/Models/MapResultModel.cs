namespace MapsRepositoryService.Core.Models;

public record MapResultModel(bool Success, ImageBase64FileModel? MapFileAsBase64String, string ErrorMessage = "");