using System.Reactive.Linq;
using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Infrastructure.MinIo;
using Microsoft.Extensions.Logging;
using Minio;

namespace MapsRepositoryService.Infrastructure.Repositories;

internal class MapsRepository : IMapsRepository
{
    private readonly ILogger<MapsRepository> _logger;
    private readonly MinioClient _minIoClient;
    private const string BucketName = "mapsdb";
    private const string MissionMapBucketName = "missiondb";

    public MapsRepository(ILogger<MapsRepository> logger, IMinIoClientBuilder minIoClientBuilder)
    {
        _logger = logger;
        _minIoClient = minIoClientBuilder.Build(BucketName, MissionMapBucketName);
    }
    
    public async Task<IList<MapListItemModel>> GetAllMapsAsync()
    {
        IDisposable? subscription = null;
        var result = new List<MapListItemModel>();
        try
        {
            var listArgs = new ListObjectsArgs()
                .WithBucket(BucketName);
            
            var isNotEmpty = await _minIoClient.ListObjectsAsync(listArgs).Any();
            if (isNotEmpty)
            {
                var queryResult = await _minIoClient.ListObjectsAsync(listArgs).ToList();

                var missionMap = await GetMissionMapNameAsync();

                foreach (var item in queryResult)
                {
                    var map = new MapListItemModel
                    {
                        MapName = item.Key
                    };

                    if (string.IsNullOrWhiteSpace(missionMap) is false)
                    {
                        map.IsMissionMap = map.MapName.Equals(missionMap);
                    }

                    result.Add(map);
                }
            }
        }
        catch (Exception e)
        {
            // this is workaround fix to _bug in MinIo nuget package
            // ListObjectsAsync.ToList() on empty bucket lead to Crash 
            if (e.Message == $"MinIO API responded with message=Bucket {BucketName} is empty.")
                return result;

            const string errorMessage = "GetAllMapsAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
        finally
        {
            subscription?.Dispose();
        }

        return result;
    }

    public async Task<MapResultModel> GetMapByNameAsync(string mapFileName)
    {
        try
        {
            var bytes = Array.Empty<byte>();

            var args = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(mapFileName)
                .WithCallbackStream(stream =>
                {
                    using var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                });

            var stat = await _minIoClient.GetObjectAsync(args);
            var ext = Path.GetExtension(stat.ObjectName).Replace(".", "");
            var result = new MapResultModel
            {
                ImageMetaData = $"data:image/{ext};base64",
                ImageBase64 = Convert.ToBase64String(bytes)
            };
            return result;
        }
        catch (Exception e)
        {
            const string errorMessage = "GetMapByNameAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    public async Task AddMapAsync(MapFileModel mapFileModel)
    {
        try
        {
            var args = new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(mapFileModel.FileName)
                .WithStreamData(mapFileModel.MapFile)
                .WithObjectSize(mapFileModel.MapFile!.Length)
                .WithContentType("application/octet-stream");
     
            await _minIoClient.PutObjectAsync(args);
        }
        catch (Exception e)
        {
            const string errorMessage = "AddMapAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    public async Task DeleteMapAsync(string mapFileName)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(mapFileName);

            await _minIoClient.RemoveObjectAsync(args);
        }
        catch (Exception e)
        {
            const string errorMessage = "AddMapAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    public async Task SetMissionMapAsync(string mapName)
    {
        try
        {
            var previousMissionMap = await GetMissionMapNameAsync();
            if (string.IsNullOrWhiteSpace(previousMissionMap) is false)
                await RemoveMissionMapAsync(previousMissionMap);

            var cpSrcArgs = new CopySourceObjectArgs()
                .WithBucket(BucketName)
                .WithObject(mapName);

            var args = new CopyObjectArgs()
                .WithBucket(MissionMapBucketName)
                .WithObject(mapName)
                .WithCopyObjectSource(cpSrcArgs);

            await _minIoClient.CopyObjectAsync(args);
        }
        catch (Exception e)
        {
            const string errorMessage = "SetMissionMapAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
    
    public async Task<MapResultModel> GetMissionMapAsync()
    {
        try
        {
            var missionMap = await GetMissionMapNameAsync();
            if (string.IsNullOrWhiteSpace(missionMap))
                return new MapResultModel();

            return await GetMapByNameAsync(missionMap);
        }
        catch (Exception e)
        {
            const string errorMessage = "GetMapByNameAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    private async Task RemoveMissionMapAsync(string mapName)
    {
        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(MissionMapBucketName)
                .WithObject(mapName);
            await _minIoClient.RemoveObjectAsync(args);
        }
        catch (Exception e)
        {
            const string errorMessage = "RemoveMissionMapAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }

    private async Task<string> GetMissionMapNameAsync()
    {
        var result = string.Empty;
        try
        {
            var listArgs = new ListObjectsArgs()
                .WithBucket(MissionMapBucketName);

            var queryResult = await _minIoClient.ListObjectsAsync(listArgs).FirstOrDefaultAsync();
            result = queryResult?.Key;
            return result ?? string.Empty;
        }
        catch (Exception e)
        {
            // this is workaround fix to _bug in MinIo nuget package
            // ListObjectsAsync.ToList() on empty bucket lead to Crash 
            if (e.Message == $"MinIO API responded with message=Bucket {MissionMapBucketName} is empty.")
                return result ?? string.Empty;

            const string errorMessage = "GetMissionMapNameAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
}