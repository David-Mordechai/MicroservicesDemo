using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : ControllerBase
{
    public record UploadMapModel(string? FileName, IFormFile File);
    
    private readonly IMapService _mapService;

    public MapsController(IMapService mapService)
    {
        _mapService = mapService;
    }

    [HttpGet]
    public async Task<IList<MapListItemModel>> Get()
    {
        return await _mapService.GetAllMapsAsync();
    }

    [HttpGet("{mapFileName}")]
    public async Task<MapResultModel> Get(string mapFileName)
    {
        return await _mapService.GetMapByNameAsync(mapFileName);
    }

    [HttpPost]
    public async Task<UploadMapResultModel> Post([FromForm] UploadMapModel uploadMapModel)
    {
        var (fileName, formFile) = uploadMapModel;
        var fileExtension = Path.GetExtension(formFile.FileName);
        var mapFileModel = new MapFileModel
        {
            File = formFile.OpenReadStream(),
            FileName = $"{fileName}{fileExtension}",
            FileExtension = fileExtension
        };
        return await _mapService.UploadMapAsync(mapFileModel);
    }
    
    [HttpDelete("{mapFileName}")]
    public async Task<ResultModel> Delete(string mapFileName)
    {
        return await _mapService.DeleteMapAsync(mapFileName);
    }
}