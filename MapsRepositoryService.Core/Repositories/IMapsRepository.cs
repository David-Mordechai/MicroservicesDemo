using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Repositories;

public interface IMapsRepository
{
    IList<string> GetAllMaps();
    MapFileModel GetMapByName(string mapName);
    void AddMap(MapFileModel mapFileModel);
    string DeleteMap(string mapName);
}