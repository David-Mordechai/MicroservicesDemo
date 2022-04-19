using MapsRepositoryService.Core.Validation.Validators.Interfaces;

namespace MapsRepositoryService.Core.Validation.Validators;

public class FileNameValidator : IFileNameValidator
{
    public (bool Valid, string ErrorMessage) IsNotEmpty(string? fileName)
    {
        return string.IsNullOrWhiteSpace(fileName) ? 
            (false, "File name is required.") : 
            (true, string.Empty);
    }
}