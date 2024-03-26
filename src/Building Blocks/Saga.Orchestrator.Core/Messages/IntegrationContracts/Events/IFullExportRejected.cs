using Saga.Orchestrator.Core.DomainObjects;
using System.Text.Json.Serialization;

namespace Saga.Orchestrator.Core.Messages.IntegrationContracts.Events
{
    public interface IFullExportRejected
    {
        DateTime Timestamp { get; }
        Cpf Cpf { get; }
        string Reason { get; }
    }
}
