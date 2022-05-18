using MapEntitiesService.Core.Models;
using MessageBroker.Core.Models;

namespace MapEntitiesService.Core.Services.Interfaces;

public interface IMapEntityService
{
    Task<ResultModel> ProcessMapEntity(MapEntity mapEntity);
}