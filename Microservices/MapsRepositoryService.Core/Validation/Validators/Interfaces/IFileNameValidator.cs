namespace MapsRepositoryService.Core.Validation.Validators.Interfaces;

public interface IFileNameValidator
{
    (bool Valid, string ErrorMessage) IsNotEmpty(string? fileName);
    (bool Valid, string ErrorMessage) IsUnique(string fileName, string fileExtension);
    (bool Valid, string ErrorMessage) IsValid(string fileName);
}