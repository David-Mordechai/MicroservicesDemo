namespace MapsRepositoryService.Core.Validation.Validators.Interfaces;

public interface IFileValidator
{
    (bool Valid, string ErrorMessage) IsNotEmpty(Stream? file);
    (bool Valid, string ErrorMessage) IsFileSizeValid(Stream file);
}