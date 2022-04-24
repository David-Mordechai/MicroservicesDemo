namespace MapsRepositoryService.Core.Validation.Validators.Interfaces;

public interface IFileExtensionValidator
{
    (bool Valid, string ErrorMessage) IsFileExtensionValid(string fileExtenstion);
}