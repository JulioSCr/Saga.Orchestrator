using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands
{
    public interface ISubmitFullExport
    {
        Cpf Cpf { get; }
        DateTime Timestamp { get; }
    }
}
