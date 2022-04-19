using MapsRepositoryService.Core.Validation.Validators.Interfaces;

namespace MapsRepositoryService.Core.Validation.Validators;

public class FileValidator : IFileValidator
{
    public (bool Valid, string ErrorMessage) IsNotEmpty(Stream? file)
    {
        return file is null ? 
            (false, "File is required.") : 
            (true, string.Empty);
    }

    public (bool Valid, string ErrorMessage) IsFileSizeValid(Stream file)
    {
        const int oneMb = 1024; 
        var fileSizeInKbs = file.Length / 1024;
        return fileSizeInKbs > oneMb ? 
            (false, $"File size has to be less then 1024 Kbs, actual file size is {fileSizeInKbs} Kbs.") : 
            (true, string.Empty);
    }
}