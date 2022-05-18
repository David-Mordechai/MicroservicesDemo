using MapsRepositoryService.Core.Configurations;
using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Core.Services.Interfaces;
using MessageBroker.Core;
using Microsoft.Extensions.Logging;

namespace MapsRepositoryService.Core.Services;

public class MapMissionService : IMapMissionService
{
    private readonly ILogger<MapMissionService> _logger;
    private readonly IMapsRepository _mapsRepository;
    private readonly IPublisher _publisher;
    private readonly Settings _settings;

    public MapMissionService(ILogger<MapMissionService> logger, 
        IMapsRepository mapsRepository, IPublisher publisher, Settings settings)
    {
        _logger = logger;
        _mapsRepository = mapsRepository;
        _publisher = publisher;
        _settings = settings;
    }

    public async Task<MapResultModel> GetMissionMapAsync()
    {
        try
        {
            var result = await _mapsRepository.GetMissionMapAsync();
            return new MapResultModel(Success: true, MapFileAsBase64String: result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "MapMissionController Get map by map name failed: {errorMessage}", e.Message);
            return new MapResultModel(Success: false, MapFileAsBase64String: null, ErrorMessage: "Mission map not found");
        }
    }

    public async Task<ResultModel> SetMissionMapAsync(string mapName)
    {
        if (string.IsNullOrWhiteSpace(mapName))
            return new ResultModel(Success: false, ErrorMessage: "Map name is required!");

        try
        {
            await _mapsRepository.SetMissionMapAsync(mapName);
            var result = await _publisher.Publish(mapName, _settings.NewMissionMapTopic);
            return new ResultModel(Success: result.Success, ErrorMessage: result.ErrorMessage);
        }
        catch (Exception e)
        {
            var errorMessage = $"Fail to set mission map: {mapName}!";
            _logger.LogError(e, "MapsController, Set Mission Map method failed: {errorMessage}", errorMessage);
            return new ResultModel(Success: false, ErrorMessage: errorMessage);
        }
    }
}