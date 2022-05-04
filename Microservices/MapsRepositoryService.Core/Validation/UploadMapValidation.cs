using MapsRepositoryService.Core.Validation.Interfaces;
using MapsRepositoryService.Core.Validation.Validators.Interfaces;

namespace MapsRepositoryService.Core.Validation;

public class UploadMapValidation : IUploadMapValidation
{
    private readonly IFileNameValidator _fileNameValidator;
    private readonly IFileValidator _fileValidator;
    private readonly IFileExtensionValidator _fileExtensionValidator;

    public UploadMapValidation(
        IFileNameValidator fileNameValidator,
        IFileValidator fileValidator,
        IFileExtensionValidator fileExtensionValidator)
    {
        _fileNameValidator = fileNameValidator;
        _fileValidator = fileValidator;
        _fileExtensionValidator = fileExtensionValidator;
    }

    public (bool Valid, string ErrorMessage) ValidateFileName(string? fileName, string fileExtension)
    {
        var validationResult = _fileNameValidator.IsNotEmpty(fileName);
        if (validationResult.Valid is false) return validationResult;

        validationResult = _fileNameValidator.IsValid(fileName!);
        if (validationResult.Valid is false) return validationResult;

        validationResult = _fileNameValidator.IsUnique(fileName!, fileExtension);
        if (validationResult.Valid is false) return validationResult;

        return validationResult.Valid is false ? validationResult : 
            (Valid: true, ErrorMessage: string.Empty);
    }

    public (bool Valid, string ErrorMessage) ValidateFile(string? fileExtension, Stream? file)
    {
        var validationResult = _fileValidator.IsNotEmpty(file);
        if (validationResult.Valid is false) return validationResult;

        validationResult = _fileValidator.IsFileSizeValid(file!);
        if (validationResult.Valid is false) return validationResult;

        validationResult = _fileExtensionValidator.IsFileExtensionValid(fileExtension!);

        return validationResult.Valid is false ? validationResult :
            (Valid: true, ErrorMessage: string.Empty);
    }
}