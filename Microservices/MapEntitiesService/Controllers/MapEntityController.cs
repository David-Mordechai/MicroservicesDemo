using MapEntitiesService.Core.Models;
using MapEntitiesService.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MapEntitiesService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapEntityController : ControllerBase
{
    private readonly IMapEntityService _mapEntityService;

    public MapEntityController(IMapEntityService mapEntityService)
    {
        _mapEntityService = mapEntityService;
    }

    [HttpPost]
    public async Task<ResultModel> Post([FromBody]MapEntity mapEntity)
    {
        return await _mapEntityService.ProcessMapEntity(mapEntity);
    }
}