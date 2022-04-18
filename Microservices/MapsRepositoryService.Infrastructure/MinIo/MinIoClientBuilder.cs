using MapsRepositoryService.Infrastructure.MinIo.Configuration;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.Exceptions;

namespace MapsRepositoryService.Infrastructure.MinIo;

internal class MinIoClientBuilder : IMinIoClientBuilder
{
    private readonly ILogger<MinIoClientBuilder> _logger;
    private readonly MinIoConfiguration _configuration;


    public MinIoClientBuilder(ILogger<MinIoClientBuilder> logger, MinIoConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public MinioClient Build(string bucketName)
    {
        try
        {
            var minIoClient = new MinioClient()
                .WithEndpoint(_configuration.BootstrapServers)
                .WithCredentials(_configuration.RootUser, _configuration.RootPassword)
                .Build();

            if (minIoClient is null)
                throw new InvalidOperationException("minIoClient is null");

            CreateMapsBucket(minIoClient, bucketName).ConfigureAwait(false).GetAwaiter().GetResult();

            return minIoClient;
        }
        catch (Exception e)
        {
            const string errorMessage = "Creation of MinIO client failed!";
            _logger.LogError(e, errorMessage);
            throw new InvalidOperationException(errorMessage);
        }
    }
    private async Task CreateMapsBucket(IBucketOperations bucketOperations, string bucketName)
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            // Create bucket if it doesn't exist.
            var found = await bucketOperations.BucketExistsAsync(bucketExistsArgs);
            if (found is false)
            {
                var meBucketArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await bucketOperations.MakeBucketAsync(meBucketArgs);
            }
        }
        catch (MinioException e)
        {
            _logger.LogError(e, "MapsBucket does not exists or creation failed!");
        }
    }
}