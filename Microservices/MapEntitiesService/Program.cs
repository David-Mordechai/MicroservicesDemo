using MapEntitiesService.Core.Configurations;
using MapEntitiesService.Infrastructure;
using MessageBroker.Infrastructure;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((hostBuilderContext, loggerConfiguration) => loggerConfiguration
        .WriteTo.Console()
        .ReadFrom.Configuration(hostBuilderContext.Configuration));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
    builder.Services.AddMapEntityServiceInfrastructure(settings);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
