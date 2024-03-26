using Saga.Orchestrator.Worker.Configuration;
using Saga.Orchestrator.Worker.Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, config) =>
    {
        config.AddJsonFile("appsettings.json", false, true);
        config.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", false, true);
        config.AddEnvironmentVariables();
        if (args?.Length > 0)
            config.AddCommandLine(args);
    })
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();


static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
{
    services.RegisterServices();
    services.AddPostgresConfiguration(ctx.Configuration);
    services.AddMassTransitConfiguration();
    services.AddHostedService<MassTransitConsoleHostedService>();
}
