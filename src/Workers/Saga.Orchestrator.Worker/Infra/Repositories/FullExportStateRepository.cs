using Microsoft.EntityFrameworkCore;
using Saga.Orchestrator.Worker.Infra.Contexts;
using Saga.Orchestrator.Worker.Infra.Interfaces;

namespace Saga.Orchestrator.Worker.Infra.Repositories
{
    public sealed class FullExportStateRepository : IFullExportStateRepository
    {
        private readonly FullExportStateDbContext _context;

        public FullExportStateRepository(FullExportStateDbContext context)
        {
            _context = context;
        }

        public async Task<int> CountByCpfAndNotInState(string cpf, params string[] state)
        {
            var stateQuery = _context.FullExportStates.AsQueryable();

            var total = await stateQuery
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.Cpf == cpf && !state.Contains(x.CurrentState))
                .CountAsync();

            return total;
        }
    }
}
