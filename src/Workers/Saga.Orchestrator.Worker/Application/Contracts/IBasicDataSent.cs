using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Worker.Application.Contracts
{
    public interface IBasicDataSent
    {
        Guid ExportId { get; }
        DateTime Timestamp { get; }
        Cpf Cpf { get; }
    }
}
