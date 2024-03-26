using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Saga.Orchestrator.Core.Configuration.DatabaseFlavor
{
    public static class ContextConfiguration
    {
        public static IServiceCollection Persist<TContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> dbConfig) where TContext : DbContext
        {
            if (services.All<ServiceDescriptor>((Func<ServiceDescriptor, bool>)(x => x.ServiceType != typeof(TContext))))
            {
                services.AddDbContext<TContext>(dbConfig);
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            }

            return services;
        }
    }
}
