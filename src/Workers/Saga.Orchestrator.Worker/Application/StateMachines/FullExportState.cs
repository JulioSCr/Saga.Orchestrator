using MassTransit;
using MongoDB.Bson.Serialization.Attributes;
using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Worker.Application.StateMachines
{
    public sealed class FullExportState :
        SagaStateMachineInstance,
        ISagaVersion
    {
        public string? CurrentState { get; set; }
        public Cpf? Cpf { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }
        public int Version { get; set; }
        [BsonId]
        public Guid CorrelationId { get; set; }
    }
}
