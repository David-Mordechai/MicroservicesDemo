using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Repositories;

public interface IMapsRepository
{
    Task<IList<MapObjectModel>> GetAllMapsAsync();
    Task<MapFileModel> GetMapByNameAsync(string mapFileName);
    Task AddMapAsync(MapFileModel mapFileModel);
    string DeleteMap(string mapFileName);
}