using Aero.Core.Logger;
using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : ControllerBase
{
    public record ViewModel(string? MapName, IFormFile? File);
    public record ResultModel(bool Success, MapFileModel? MapFileModel, string ErrorMessage = "" );

    private readonly IAeroLogger<MapsController> _logger;
    private readonly IMapsRepository _mapsRepository;

    public MapsController(IAeroLogger<MapsController> logger, IMapsRepository mapsRepository)
    {
        _logger = logger;
        _mapsRepository = mapsRepository;
    }

    [HttpGet]
    public IList<string> Get()
    {
        return _mapsRepository.GetAllMaps();
    }

    [HttpGet("{mapName}")]
    public ResultModel Get(string mapName)
    {
        if (string.IsNullOrWhiteSpace(mapName))
            return new ResultModel(Success: false, MapFileModel: null, ErrorMessage: "Map name is required");

        try
        {
            var result = _mapsRepository.GetMapByName(mapName);
            return new ResultModel(Success: true, MapFileModel: result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ResultModel(Success: false, MapFileModel: null, ErrorMessage: $"Map {mapName} not found");
        }
    }

    [HttpPost]
    public string Post([FromForm] ViewModel viewModel)
    {
        var (mapName, formFile) = viewModel;
        if (string.IsNullOrWhiteSpace(mapName))
            return "File name is required";

        if (formFile is null)
            return "File is required";

        try
        {
            var fileMemoryStream = new MemoryStream();
            formFile.OpenReadStream().CopyTo(fileMemoryStream);
            var mapFileModel = new MapFileModel
            {
                MapName = mapName,
                MapFile = fileMemoryStream
            };

            _mapsRepository.AddMap(mapFileModel);
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to upload {mapName} file!";
            _logger.LogError(errorMessage, e);
            return errorMessage;
        }
        
        return "File uploaded";
    }
    

    [HttpDelete("{mapName}")]
    public string Delete(string mapName)
    {
        if (string.IsNullOrWhiteSpace(mapName))
            return "Map name is required!";

        try
        {
            return _mapsRepository.DeleteMap(mapName);
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to delete {mapName} file!";
            _logger.LogError(errorMessage, e);
            return errorMessage;
        }
    }
}