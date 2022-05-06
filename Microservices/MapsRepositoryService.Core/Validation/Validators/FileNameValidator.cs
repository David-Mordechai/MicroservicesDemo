using System.Text.RegularExpressions;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Core.Validation.Validators.Interfaces;

namespace MapsRepositoryService.Core.Validation.Validators;

public class FileNameValidator : IFileNameValidator
{
    private readonly IMapsRepository _mapsRepository;

    public FileNameValidator(IMapsRepository mapsRepository)
    {
        _mapsRepository = mapsRepository;
    }
    public (bool Valid, string ErrorMessage) IsNotEmpty(string? fileName)
    {
        return string.IsNullOrWhiteSpace(fileName) ? 
            (false, "File name is required.") : 
            (true, string.Empty);
    }

    public (bool Valid, string ErrorMessage) IsUnique(string fileName, string fileExtension)
    {
        var fullFileName = $"{fileName}{fileExtension}";
        var fileNameExists = _mapsRepository.IsExistsAsync(fullFileName).GetAwaiter().GetResult();

        return fileNameExists ? 
            (false, $"File Name {fullFileName} already exists.") : 
            (true, string.Empty);
    }

    public (bool Valid, string ErrorMessage) IsValid(string fileName, string fileExtension)
    {
        var fileNameWithoutFileExtension = fileName.Replace(fileExtension, string.Empty);
        var regex = new Regex("^[A-Za-z0-9]*$");
        var isMatch = regex.IsMatch(fileNameWithoutFileExtension);
        return isMatch ? 
            (true, string.Empty) : 
            (false, "Only letters or numbers allowed.");
    }
}