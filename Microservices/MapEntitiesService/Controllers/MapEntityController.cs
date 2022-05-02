using MapEntitiesService.Configurations;
using MessageBroker.Core;
using Microsoft.AspNetCore.Mvc;

namespace MapEntitiesService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapEntityController : ControllerBase
{
    private readonly ILogger<MapEntityController> _logger;
    private readonly Settings _settings;

    public MapEntityController(ILogger<MapEntityController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _settings = configuration.GetSection("Settings").Get<Settings>();
    }

    public record MapEntity(string Title, double XPosition, double YPosition, double MapWidth, double MapHeight);

    [HttpPost]
    public IActionResult Post([FromServices] IPublisher publisher, [FromBody]MapEntity mapEntity)
    {
        _logger.LogInformation("Publishing new MapEntity - {mapEntity}", mapEntity);
        publisher.Publish(mapEntity, topic: _settings.NewMapEntityTopic);
        return Ok();
    }
}