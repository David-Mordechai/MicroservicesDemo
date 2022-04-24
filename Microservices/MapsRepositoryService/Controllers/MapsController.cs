using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Core.Validation.Interfaces;
using MessageBroker.Core;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : ControllerBase
{
    public record ResultModel(bool Success, string MapFileAsBase64String, string ErrorMessage = "" );
    public record UploadMapViewModel(string? FileName, IFormFile? File);

    private readonly ILogger<MapsController> _logger;
    private readonly IMapsRepository _mapsRepository;
    private readonly IUploadMapValidation _uploadMapValidation;
    private readonly IPublisher _publisher;

    public MapsController(
        ILogger<MapsController> logger, 
        IMapsRepository mapsRepository,
        IUploadMapValidation uploadMapValidation,
        IPublisher publisher)
    {
        _logger = logger;
        _mapsRepository = mapsRepository;
        _uploadMapValidation = uploadMapValidation;
        _publisher = publisher;
    }

    [HttpGet]
    public async Task<IList<string>> Get()
    {
        return await _mapsRepository.GetAllMapsAsync();
    }

    [HttpGet("{mapFileName}")]
    public async Task<ResultModel> Get(string mapFileName)
    {
        if (string.IsNullOrWhiteSpace(mapFileName))
            return new ResultModel(Success: false, MapFileAsBase64String: "", ErrorMessage: "Map name is required");

        try
        {
            var result = await _mapsRepository.GetMapByNameAsync(mapFileName);
            return new ResultModel(Success: true, MapFileAsBase64String: result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "MapsController Get map by map name failed: {errorMessage}", e.Message);
            return new ResultModel(Success: false, MapFileAsBase64String: "", ErrorMessage: $"Map {mapFileName} not found");
        }
    }

    [HttpPost]
    public async Task<string> Post([FromForm] UploadMapViewModel uploadMapViewModel)
    {
        try
        {
            var (fileName, file) = uploadMapViewModel;
            var fileExtension = Path.GetExtension(file?.FileName);
            var fileStream = file?.OpenReadStream();

            var (valid, errorMessage) = _uploadMapValidation.Validate(fileName, fileExtension, fileStream);
            if (valid is false)
                return errorMessage;

            var mapFileModel = new MapFileModel
            {
                FileName = $"{fileName}{fileExtension}",
                MapFile = fileStream
            };

            await _mapsRepository.AddMapAsync(mapFileModel);
            await _publisher.Publish(mapFileModel.FileName, "NewMapUploaded");
        }
        catch (Exception e)
        {
            const string errorMessage = "Fail to upload!";
            _logger.LogError(e, "MapsController, upload new map failed: {errorMessage}", errorMessage);
            return errorMessage;
        }
        
        return "File uploaded!";
    }
    
    [HttpDelete("{mapFileName}")]
    public async Task<string> Delete(string mapFileName)
    {
        if (string.IsNullOrWhiteSpace(mapFileName))
            return "Map name is required!";

        try
        {
            await _mapsRepository.DeleteMapAsync(mapFileName);
            return "Map deleted successfully!";
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to delete {mapFileName} file!";
            _logger.LogError(e, "MapsController, Map delete failed: {errorMessage}", errorMessage);
            return errorMessage;
        }
    }
}