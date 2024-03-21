using Saga.Orchestrator.Worker.Configuration;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(ConfigureServices)
    .Build();

await host.RunAsync();

static void ConfigureServices(IServiceCollection services)
{
    services.RegisterServices();

    services.AddMassTransitConfiguration();
}
