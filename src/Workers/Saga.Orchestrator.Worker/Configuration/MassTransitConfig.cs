using MassTransit;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Saga.Orchestrator.Worker.Application.BatchConsumers;
using Saga.Orchestrator.Worker.Application.Consumers;
using Saga.Orchestrator.Worker.Application.StateMachines;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands;

namespace Saga.Orchestrator.Worker.Configuration
{
    public static class MassTransitConfig
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<SubmitFullExportConsumer>();

                cfg.AddSagaStateMachine<FullExportStateMachine, FullExportState>(typeof(FullExportStateMachineDefinition))
                    .RedisRepository(s => s.DatabaseConfiguration("localhost:6379"));

                cfg.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ReceiveEndpoint(KebabCaseEndpointNameFormatter.Instance.Consumer<RoutingSlipBatchEventConsumer>(), e =>
                    {
                        e.PrefetchCount = 20;

                        e.Batch<RoutingSlipCompleted>(b =>
                        {
                            b.MessageLimit = 10;
                            b.TimeLimit = TimeSpan.FromSeconds(5);

                            b.Consumer<RoutingSlipBatchEventConsumer, RoutingSlipCompleted>(ctx);
                        });
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
