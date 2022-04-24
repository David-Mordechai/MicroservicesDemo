using MapsRepositoryService.Core.Validation.Validators.Interfaces;

namespace MapsRepositoryService.Core.Validation.Validators;

public class FileExtensionValidator : IFileExtensionValidator
{
    public (bool Valid, string ErrorMessage) IsFileExtensionValid(string fileExtenstion)
    {
        var allowedExtensions = new[] {".jpeg", ".jpg", ".png", ".svg"};
        return allowedExtensions.Contains(fileExtenstion) is false ? 
            (false, "Invalid file extenstion, only .jpg, .jpeg, .png, .svg are allowed.") : 
            (true, string.Empty);
    }
}