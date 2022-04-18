using System.Reactive.Linq;
using MapsRepositoryService.Core.Models;
using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Infrastructure.MinIo;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel;

namespace MapsRepositoryService.Infrastructure.Repositories;

internal class MapsRepository : IMapsRepository
{
    private readonly ILogger<MapsRepository> _logger;
    private readonly MinioClient _minIoClient;
    private const string BucketName = "mapsdb";

    public MapsRepository(ILogger<MapsRepository> logger, IMinIoClientBuilder minIoClientBuilder)
    {
        _logger = logger;
        _minIoClient = minIoClientBuilder.Build(BucketName);
    }

    public async Task<IList<string>> GetAllMapsAsync()
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
                ex => throw new Exception(ex.Message));

            await observable;
        }
        catch (Exception e)
        {
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

    public async Task<string> GetMapByNameAsync(string mapFileName)
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
            var result = $"data:image/{ext};base64,{Convert.ToBase64String(bytes)}";
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

            Console.WriteLine("Running example for API: RemoveObjectAsync");
            await _minIoClient.RemoveObjectAsync(args);
        }
        catch (Exception e)
        {
            const string errorMessage = "AddMapAsync method filed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
}