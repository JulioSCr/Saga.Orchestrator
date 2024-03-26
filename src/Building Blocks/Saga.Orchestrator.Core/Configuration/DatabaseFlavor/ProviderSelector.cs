using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Saga.Orchestrator.Core.Configuration.DatabaseFlavor
{
    public static class ProviderSelector
    {
        public static IServiceCollection ConfigureProviderForContextPostgres<TContext>(this IServiceCollection services,
            string options) where TContext : DbContext
        {
            ProviderConfiguration.Build(options);
            return services.Persist<TContext>(ProviderConfiguration.With.Postgres);
        }
    }
}
