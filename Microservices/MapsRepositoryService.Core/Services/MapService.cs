using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Core.Services.Interfaces;
using MapsRepositoryService.Core.Validation.Interfaces;
using Microsoft.Extensions.Logging;

namespace MapsRepositoryService.Core.Services;

public class MapService : IMapService
{
    private readonly ILogger<MapService> _logger;
    private readonly IMapsRepository _mapsRepository;
    private readonly IUploadMapValidation _uploadMapValidation;

    public MapService(ILogger<MapService> logger, 
        IMapsRepository mapsRepository, IUploadMapValidation uploadMapValidation)
    {
        _logger = logger;
        _mapsRepository = mapsRepository;
        _uploadMapValidation = uploadMapValidation;
    }

    public async Task<IList<MapListItemModel>> GetAllMapsAsync()
    {
        return await _mapsRepository.GetAllMapsAsync();
    }

    public async Task<MapResultModel> GetMapByNameAsync(string mapName)
    {
        if (string.IsNullOrWhiteSpace(mapName))
            return new MapResultModel(Success: false, MapFileAsBase64String: null, ErrorMessage: "Map name is required");

        try
        {
            var result = await _mapsRepository.GetMapByNameAsync(mapName);
            return new MapResultModel(Success: true, MapFileAsBase64String: result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "MapsController Get map by map name failed: {ErrorMessage}", e.Message);
            return new MapResultModel(Success: false, MapFileAsBase64String: null, ErrorMessage: $"Map {mapName} not found");
        }
    }

    public async Task<UploadMapResultModel> UploadMapAsync(MapFileModel mapFileModel)
    {
        try
        {
            var validationResult = _uploadMapValidation.Validate(mapFileModel);
            if (validationResult.Success is false) return validationResult;

            await _mapsRepository.AddMapAsync(mapFileModel);
            return new UploadMapResultModel(Success: true);
        }
        catch (Exception e)
        {
            const string errorMessage = "Fail to upload!";
            _logger.LogError(e, "MapsController, upload new map failed: {ErrorMessage}", errorMessage);
            return new UploadMapResultModel(false, errorMessage);
        }
    }

    public async Task<ResultModel> DeleteMapAsync(string mapName)
    {
        if (string.IsNullOrWhiteSpace(mapName))
            return new ResultModel(Success: false, ErrorMessage: "Map name is required!");

        try
        {
            await _mapsRepository.DeleteMapAsync(mapName);
            return new ResultModel(Success:true);
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to delete {mapName} file!";
            _logger.LogError(e, "MapsController, Map delete failed: {ErrorMessage}", errorMessage);
            return new ResultModel(Success: false, ErrorMessage: errorMessage); 
        }
    }
}