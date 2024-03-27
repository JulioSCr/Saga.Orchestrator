using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Core.Messages.Integration.Events
{
    public class FullExportAcceptedEvent : Event
    {
        public Guid ExportId { get; set; }
        public DateTime Timestamp { get; set; }
        public Cpf Cpf { get; set; }
    }
}
