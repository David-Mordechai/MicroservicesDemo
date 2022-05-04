namespace MapsRepositoryService.Core.Validation.Interfaces;

public interface IUploadMapValidation
{
    (bool Valid, string ErrorMessage) ValidateFile(string? fileExtension, Stream? file);
    (bool Valid, string ErrorMessage) ValidateFileName(string? fileName, string fileExtension);
}