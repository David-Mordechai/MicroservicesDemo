namespace MapsRepositoryService.Core.Validation.Validators.Interfaces;

public interface IFileNameValidator
{
    (bool Valid, string ErrorMessage) IsNotEmpty(string? fileName);
}