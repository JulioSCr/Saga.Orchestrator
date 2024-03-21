using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Worker.Application.Events
{
    public interface IBasicDataSent
    {
        Guid ExportId { get; }
        DateTime Timestamp { get; }
        Cpf Cpf { get; }
    }
}
