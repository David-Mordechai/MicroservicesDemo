using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((_, configurationBuilder) =>
{
    configurationBuilder.AddJsonFile("ocelot.json");
});
builder.Services.AddSignalR();
builder.Services.AddOcelot();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

app.UseWebSockets();
await app.UseOcelot();

app.Run();
