using MapsRepositoryService.Core.Repositories;
using MapsRepositoryService.Core.Services;
using MapsRepositoryService.Core.Services.Interfaces;
using MapsRepositoryService.Core.Validation;
using MapsRepositoryService.Core.Validation.Interfaces;
using MapsRepositoryService.Core.Validation.Validators;
using MapsRepositoryService.Core.Validation.Validators.Interfaces;
using MapsRepositoryService.Infrastructure.MinIo;
using MapsRepositoryService.Infrastructure.MinIo.Configuration;
using MapsRepositoryService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MapsRepositoryService.Infrastructure;

public static class ServicesCollectionExtension
{
    public static void AddMapsRepositoryServiceInfrastructure(this IServiceCollection services, MinIoConfiguration minIoConfiguration)
    {
        services.AddScoped<IMapsRepository, MapsRepository>();
        services.AddScoped(_ => minIoConfiguration);
        services.AddScoped<IMinIoClientBuilder, MinIoClientBuilder>();
        
        services.AddScoped<IFileExtensionValidator, FileExtensionValidator>();
        services.AddScoped<IFileValidator, FileValidator>();
        services.AddScoped<IFileNameValidator, FileNameValidator>();
        services.AddScoped<IUploadMapValidation, UploadMapValidation>();

        services.AddScoped<IMapService, MapService>();
        services.AddScoped<IMapMissionService, MapMissionService>();
    }
}