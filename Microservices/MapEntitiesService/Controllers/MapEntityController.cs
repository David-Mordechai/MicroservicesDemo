using MessageBroker.Core;
using Microsoft.AspNetCore.Mvc;

namespace MapEntitiesService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapEntityController : ControllerBase
{
    private readonly ILogger<MapEntityController> _logger;

    public MapEntityController(ILogger<MapEntityController> logger)
    {
        _logger = logger;
    }

    public record MapEntity(string Title, double XPosition, double YPosition);

    [HttpPost]
    public IActionResult Post([FromServices] IPublisher publisher, [FromBody]MapEntity mapEntity)
    {
        _logger.LogInformation("Publishing new MapEntity - {mapEntity}", mapEntity);
        publisher.Publish(mapEntity, topic: "NewMapEntity");
        return Ok();
    }
}