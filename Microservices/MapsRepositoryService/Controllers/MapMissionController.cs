using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapMissionController : ControllerBase
{
    private readonly IMapMissionService _mapMissionService;

    public record SetMissionMapModel(string MapName);

    public MapMissionController(IMapMissionService mapMissionService)
    {
        _mapMissionService = mapMissionService;
    }

    [HttpGet]
    public async Task<MapResultModel> Get()
    {
        return await _mapMissionService.GetMissionMapAsync();
    }

    [HttpPost]
    public async Task<ResultModel> Post([FromBody] SetMissionMapModel setMissionMapModel)
    {
        return await _mapMissionService.SetMissionMapAsync(setMissionMapModel.MapName);
    }
}