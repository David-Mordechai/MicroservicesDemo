using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Services.Interfaces;

public interface IMapService
{
    Task<IList<MapListItemModel>> GetAllMapsAsync();
    Task<MapResultModel> GetMapByNameAsync(string mapName);
    Task<UploadMapResultModel> UploadMapAsync(MapFileModel mapFileModel);
    Task<ResultModel> DeleteMapAsync(string mapName);
}