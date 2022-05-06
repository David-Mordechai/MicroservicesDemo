using MapEntitiesService.Core.Models;

namespace MapEntitiesService.Core.Services.Interfaces;

public interface IMapEntityService
{
    void ProcessMapEntity(MapEntity mapEntity);
}