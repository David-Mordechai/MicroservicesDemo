namespace MapsRepositoryService.Core.Validation.Interfaces;

public interface IUploadMapValidation
{
    (bool Valid, string ErrorMessage) Validate(string? fileName, string? fileExtension, Stream? file);
}