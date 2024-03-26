namespace Saga.Orchestrator.Worker.Infra.Interfaces
{
    public interface IFullExportStateRepository
    {
        Task<int> CountByCpfAndNotInState(string cpf, params string[] state);
    }
}
