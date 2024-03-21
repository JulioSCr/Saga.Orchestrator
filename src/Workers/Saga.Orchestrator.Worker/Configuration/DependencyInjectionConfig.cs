using Saga.Orchestrator.Worker.Application.Activities;
using Saga.Orchestrator.Worker.Application.BatchConsumers;
using Saga.Orchestrator.Worker.Services;

namespace Saga.Orchestrator.Worker.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<SendBasicDataActivity>();
            services.AddScoped<SendComplementsActivity>();
            services.AddScoped<RoutingSlipBatchEventConsumer>();
            services.AddHostedService<MassTransitConsoleHostedService>();
        }
    }
}
