using Aero.Core.Logger;
using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Infrastructure.MinIo;
using Minio;
using Minio.DataModel;

namespace MapsRepositoryService.Infrastructure.Repositories;

internal class MapsRepository : IMapsRepository
{
    private readonly IAeroLogger<MapsRepository> _logger;
    private readonly MinioClient _minIoClient;
    private const string BucketName = "MapsBucket";

    public MapsRepository(IAeroLogger<MapsRepository> logger, IMinIoClientBuilder minIoClientBuilder)
    {
        _logger = logger;
        _minIoClient = minIoClientBuilder.Build(BucketName);
    }

    public IList<string> GetAllMaps()
    {
        IDisposable? subscription = null;
        var result = new List<string>();
        try
        {
            var listArgs = new ListObjectsArgs()
                .WithBucket(BucketName);
            
            IObservable<Item> observable = _minIoClient.ListObjectsAsync(listArgs);
            
            subscription = observable.Subscribe(
                item => result.Add(item.Key),
                ex => _logger.LogError(ex.Message, ex));
        }
        catch (Exception e)
        {
            _logger.LogError("MapsRepository => GetAllMaps Failed!", e);
        }
        finally
        {
            subscription?.Dispose();
        }

        return result;
    }

    public MapFileModel GetMapByName(string mapName)
    {
        throw new NotImplementedException();
    }

    public void AddMap(MapFileModel mapFileModel)
    {
        throw new NotImplementedException();
    }

    public string DeleteMap(string mapName)
    {
        throw new NotImplementedException();
    }
}