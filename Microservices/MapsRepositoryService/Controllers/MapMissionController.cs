using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MessageBroker.Core;
using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MapMissionController : ControllerBase
    {
        private readonly ILogger<MapsController> _logger;
        private readonly IMapsRepository _mapsRepository;
        private readonly IPublisher _publisher;

        public record ResultModel(bool Success, MapResultModel MapFileAsBase64String, string ErrorMessage = "");
        public record SetMissionMapModel(string MapName);

        public MapMissionController(
            ILogger<MapsController> logger,
            IMapsRepository mapsRepository,
            IPublisher publisher)
        {
            _logger = logger;
            _mapsRepository = mapsRepository;
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _mapsRepository.GetMissionMapAsync();
                return Ok(new ResultModel(Success: true, MapFileAsBase64String: result));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MapMissionController Get map by map name failed: {errorMessage}", e.Message);
                return Ok(new ResultModel(Success: false, MapFileAsBase64String: new MapResultModel(), ErrorMessage: "Mission map not found"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SetMissionMapModel model)
        {
            if (string.IsNullOrWhiteSpace(model.MapName))
                return BadRequest("Map name is required!");

            try
            {
                await _mapsRepository.SetMissionMapAsync(model.MapName);
                await _publisher.Publish(model.MapName, "NewMissionMap");
                return Ok();
            }
            catch (Exception e)
            {
                var errorMessage = $"Fail to delete {model.MapName} file!";
                _logger.LogError(e, "MapsController, Set Mission Map method failed: {errorMessage}", errorMessage);
                return Problem(errorMessage);
            }
        }
    }
}
