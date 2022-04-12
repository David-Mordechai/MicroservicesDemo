using System.Reactive.Linq;
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
    private const string BucketName = "mapsdb";

    public MapsRepository(IAeroLogger<MapsRepository> logger, IMinIoClientBuilder minIoClientBuilder)
    {
        _logger = logger;
        _minIoClient = minIoClientBuilder.Build(BucketName);
    }

    public async Task<IList<MapObjectModel>> GetAllMapsAsync()
    {
        IDisposable? subscription = null;
        var result = new List<MapObjectModel>();
        try
        {
            var listArgs = new ListObjectsArgs()
                .WithBucket(BucketName);
            
            IObservable<Item> observable = _minIoClient.ListObjectsAsync(listArgs);
            
            subscription = observable.Subscribe(
                item => result.Add(new MapObjectModel{Id = item.ETag, FileName = item.Key}),
                ex => _logger.LogError(ex.Message, ex));

            await observable;
        }
        catch (Exception e)
        {
            const string errorMessage = "GetAllMapsAsync method filed!";
            _logger.LogError(errorMessage, e);
            throw new InvalidOperationException(errorMessage);
        }
        finally
        {
            subscription?.Dispose();
        }

        return result;
    }

    public async Task<MapFileModel> GetMapByNameAsync(string mapFileName)
    {
        try
        {
            var result = new MapFileModel();

            var args = new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(mapFileName)
                .WithCallbackStream(stream =>
                {
                    var fileStream = File.Create(mapFileName);
                    stream.CopyToAsync(fileStream);
                    result.MapFile = fileStream;
                    //fileStream.Dispose();
                    stream.Dispose();
                });

            var stat = await _minIoClient.GetObjectAsync(args);
            result.FileName = stat.ObjectName;

            return result;
        }
        catch (Exception e)
        {
            const string errorMessage = "GetMapByNameAsync method filed!";
            _logger.LogError(errorMessage, e);
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
            _logger.LogError(errorMessage, e);
            throw new InvalidOperationException(errorMessage);
        }
    }

    public string DeleteMap(string mapFileName)
    {
        throw new NotImplementedException();
    }
}