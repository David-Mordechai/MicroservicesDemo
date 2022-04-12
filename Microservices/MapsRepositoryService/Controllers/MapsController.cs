using Aero.Core.Logger;
using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : ControllerBase
{
    public record ViewModel(string? FileName, IFormFile? File);
    public record ResultModel(bool Success, MapFileModel? MapFileModel, string ErrorMessage = "" );

    private readonly IAeroLogger<MapsController> _logger;
    private readonly IMapsRepository _mapsRepository;

    public MapsController(IAeroLogger<MapsController> logger, IMapsRepository mapsRepository)
    {
        _logger = logger;
        _mapsRepository = mapsRepository;
    }

    [HttpGet]
    public async Task<IList<MapObjectModel>> Get()
    {
        return await _mapsRepository.GetAllMapsAsync();
    }

    [HttpGet("{mapFileName}")]
    public async Task<ResultModel> Get(string mapFileName)
    {
        if (string.IsNullOrWhiteSpace(mapFileName))
            return new ResultModel(Success: false, MapFileModel: null, ErrorMessage: "Map name is required");

        try
        {
            var result = await _mapsRepository.GetMapByNameAsync(mapFileName);
            return new ResultModel(Success: true, MapFileModel: result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ResultModel(Success: false, MapFileModel: null, ErrorMessage: $"Map {mapFileName} not found");
        }
    }

    [HttpPost]
    public async Task<string> Post([FromForm] ViewModel viewModel)
    {
        var (fileName, formFile) = viewModel;
        if (string.IsNullOrWhiteSpace(fileName))
            return "File name is required";

        if (formFile is null)
            return "File is required";

        var fileExtension = Path.GetExtension(formFile.FileName);
        // Todo => validate file extension to allowed image format [jpeg, jpg, png, svg]
        // Todo => abstract validations
        
        try
        {
            var mapFileModel = new MapFileModel
            {
                FileName = $"{fileName}{fileExtension}",
                MapFile = formFile.OpenReadStream()
            };

            await _mapsRepository.AddMapAsync(mapFileModel);
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to upload {fileName} file!";
            _logger.LogError(errorMessage, e);
            return errorMessage;
        }
        
        return "File uploaded";
    }
    

    [HttpDelete("{mapFileName}")]
    public string Delete(string mapFileName)
    {
        if (string.IsNullOrWhiteSpace(mapFileName))
            return "Map name is required!";

        try
        {
            return _mapsRepository.DeleteMap(mapFileName);
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to delete {mapFileName} file!";
            _logger.LogError(errorMessage, e);
            return errorMessage;
        }
    }
}