using Minio;

namespace MapsRepositoryService.Infrastructure.MinIo;

internal interface IMinIoClientBuilder
{
    MinioClient Build(string bucketName);
}