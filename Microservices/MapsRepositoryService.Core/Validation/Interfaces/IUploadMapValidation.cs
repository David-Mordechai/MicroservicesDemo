using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Validation.Interfaces;

public interface IUploadMapValidation
{
    UploadMapResultModel Validate(MapFileModel mapFileModel);
}