using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Repositories;

public interface IMapsRepository
{
    Task<IList<MapListItemModel>> GetAllMapsAsync();
    Task<MapResultModel> GetMapByNameAsync(string mapFileName);
    Task AddMapAsync(MapFileModel mapFileModel);
    Task DeleteMapAsync(string mapFileName);

    Task SetMissionMapAsync(string mapName);
    Task<MapResultModel> GetMissionMapAsync();
}