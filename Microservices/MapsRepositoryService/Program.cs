using MapsRepositoryService.Infrastructure;
using MapsRepositoryService.Infrastructure.MinIo.Configuration;
using MessageBroker.Infrastructure;
using MessageBroker.Infrastructure.RabbitMq.Builder.Configuration;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddMapsRepositoryServiceInfrastructure(new MinIoConfiguration
    {
        BootstrapServers = builder.Configuration["MapDB:MapDbService"],
        RootUser = builder.Configuration["MapDB:MapDbRootUser"],
        RootPassword = builder.Configuration["MapDB:MapDbRootPassword"]
    });

    builder.Services.AddMessageBrokerProducerServicesRabbitMq(new RabbitMqConfiguration
    {
        BootstrapServers = builder.Configuration["brokerService"]
    });

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