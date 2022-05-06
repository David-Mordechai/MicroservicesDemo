using MapsRepositoryService.Core.Models;

namespace MapsRepositoryService.Core.Services.Interfaces;

public interface IMapMissionService
{
    Task<MapResultModel> GetMissionMapAsync();
    Task<ResultModel> SetMissionMapAsync(string mapName);
}