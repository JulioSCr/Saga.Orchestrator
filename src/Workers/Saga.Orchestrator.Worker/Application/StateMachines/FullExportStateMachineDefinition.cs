using MassTransit;

namespace Saga.Orchestrator.Worker.Application.StateMachines
{
    public sealed class FullExportStateMachineDefinition : SagaDefinition<FullExportState>
    {
        public FullExportStateMachineDefinition()
        {
            ConcurrentMessageLimit = 12;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<FullExportState> sagaConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}
