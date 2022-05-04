using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Core.Validation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : ControllerBase
{
    public record ResultModel(bool Success, MapResultModel MapFileAsBase64String, string ErrorMessage = "" );
    public record UploadMapViewModel(string? FileName, IFormFile File);
    public record UploadMapResultModel(bool Success, string ControlName = "", string ErrorMessage = "" );

    private readonly ILogger<MapsController> _logger;
    private readonly IMapsRepository _mapsRepository;
    private readonly IUploadMapValidation _uploadMapValidation;

    public MapsController(
        ILogger<MapsController> logger, 
        IMapsRepository mapsRepository,
        IUploadMapValidation uploadMapValidation)
    {
        _logger = logger;
        _mapsRepository = mapsRepository;
        _uploadMapValidation = uploadMapValidation;
    }

    [HttpGet]
    public async Task<IList<MapListItemModel>> Get()
    {
        return await _mapsRepository.GetAllMapsAsync();
    }

    [HttpGet("{mapFileName}")]
    public async Task<ResultModel> Get(string mapFileName)
    {
        if (string.IsNullOrWhiteSpace(mapFileName))
            return new ResultModel(Success: false, MapFileAsBase64String: new MapResultModel(), ErrorMessage: "Map name is required");

        try
        {
            var result = await _mapsRepository.GetMapByNameAsync(mapFileName);
            return new ResultModel(Success: true, MapFileAsBase64String: result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "MapsController Get map by map name failed: {errorMessage}", e.Message);
            return new ResultModel(Success: false, MapFileAsBase64String: new MapResultModel(), ErrorMessage: $"Map {mapFileName} not found");
        }
    }

    [HttpPost]
    public async Task<UploadMapResultModel> Post([FromForm] UploadMapViewModel uploadMapViewModel)
    {
        try
        {
            var (fileName, file) = uploadMapViewModel;
            var fileExtension = Path.GetExtension(file.FileName);
            var fileStream = file?.OpenReadStream();

            var (fileValid, fileErrorMessage) = _uploadMapValidation.ValidateFile(fileExtension, fileStream);
            if (fileValid is false)
                return new UploadMapResultModel(Success: false, nameof(uploadMapViewModel.File), fileErrorMessage);

            var (fileNameValid, fileNameErrorMessage) = _uploadMapValidation.ValidateFileName(fileName, fileExtension);
            if (fileNameValid is false)
                return new UploadMapResultModel(Success: false, nameof(uploadMapViewModel.FileName), fileNameErrorMessage);

            var mapFileModel = new MapFileModel
            {
                FileName = $"{fileName}{fileExtension}",
                MapFile = fileStream
            };

            await _mapsRepository.AddMapAsync(mapFileModel);
            return new UploadMapResultModel(Success: true);
        }
        catch (Exception e)
        {
            const string errorMessage = "Fail to upload!";
            _logger.LogError(e, "MapsController, upload new map failed: {errorMessage}", errorMessage);
            return new UploadMapResultModel(false, errorMessage);
        }
    }
    
    [HttpDelete("{mapFileName}")]
    public async Task<IActionResult> Delete(string mapFileName)
    {
        if (string.IsNullOrWhiteSpace(mapFileName))
            return BadRequest("Map name is required!");

        try
        {
            await _mapsRepository.DeleteMapAsync(mapFileName);
            return Ok();
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to delete {mapFileName} file!";
            _logger.LogError(e, "MapsController, Map delete failed: {errorMessage}", errorMessage);
            return Problem(errorMessage);
        }
    }
}