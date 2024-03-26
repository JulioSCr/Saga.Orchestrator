namespace Saga.Orchestrator.Core.Messages.Integration.Responses
{
    public interface IFullExportStatusResponse
    {
        Guid ExportId { get; }
        string State { get; }
    }
}
