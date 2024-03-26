using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands
{
    public interface ICheckStatus
    {
        Guid ExportId { get; }
    }
}
