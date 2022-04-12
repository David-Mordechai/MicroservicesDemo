using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Repositories;

public interface IMapsRepository
{
    Task<IList<string>> GetAllMapsAsync();
    Task<string> GetMapByNameAsync(string mapFileName);
    Task AddMapAsync(MapFileModel mapFileModel);
    Task DeleteMapAsync(string mapFileName);
}