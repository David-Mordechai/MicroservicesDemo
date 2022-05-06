using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Repositories;

public interface IMapsRepository
{
    Task<IList<MapListItemModel>> GetAllMapsAsync();
    Task<ImageBase64FileModel> GetMapByNameAsync(string mapFileName);
    Task AddMapAsync(MapFileModel mapFileModel);
    Task DeleteMapAsync(string mapFileName);

    Task SetMissionMapAsync(string mapName);
    Task<ImageBase64FileModel> GetMissionMapAsync();
    Task<bool> IsExistsAsync(string fileName);
}