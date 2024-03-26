using MassTransit;

namespace Saga.Orchestrator.Worker.Application.StateMachines
{
    public sealed class FullExportState :
        SagaStateMachineInstance
    {
        public string? CurrentState { get; set; }
        public string? Cpf { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }
        //public int Version { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
