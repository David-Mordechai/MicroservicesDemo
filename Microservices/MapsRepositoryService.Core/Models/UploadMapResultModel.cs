namespace MapsRepositoryService.Core.Models;

public record UploadMapResultModel(bool Success, string ControlName = "", string ErrorMessage = "");