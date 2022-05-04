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
        const int mb = 1024; 
        const int kb = 1024;
        var bytes = file.Length;
        var fileSizeInKbs = bytes / kb;
        return fileSizeInKbs > mb ? 
            (false, $"File size has to be less then 1 MB, actual file size is {fileSizeInKbs} Kbs.") : 
            (true, string.Empty);
    }
}