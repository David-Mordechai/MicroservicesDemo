using MapEntitiesService.Core.Configurations;
using MapEntitiesService.Core.Models;
using MapEntitiesService.Core.Services.Interfaces;
using MessageBroker.Core;
using Microsoft.Extensions.Logging;

namespace MapEntitiesService.Core.Services;

public class MapEntityService : IMapEntityService
{
    private readonly ILogger<MapEntityService> _logger;
    private readonly IPublisher _publisher;
    private readonly Settings _settings;

    public MapEntityService(ILogger<MapEntityService> logger, IPublisher publisher, Settings settings)
    {
        _logger = logger;
        _publisher = publisher;
        _settings = settings;
    }

    public async Task<ResultModel> ProcessMapEntity(MapEntity mapEntity)
    {
        _logger.LogInformation("Publishing new MapEntity - {MapEntity}", mapEntity);
        var result = await _publisher.Publish(mapEntity, topic: _settings.NewMapEntityTopic);
        return new ResultModel(Success: result.Success, ErrorMessage: result.ErrorMessage);
    }
}