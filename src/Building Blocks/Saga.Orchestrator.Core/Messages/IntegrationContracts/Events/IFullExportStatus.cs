namespace Saga.Orchestrator.Core.Messages.IntegrationContracts.Events
{
    public interface IFullExportStatus
    {
        Guid ExportId { get; }
        string State { get; }
    }
}
