using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Orchestrator.Core.Configuration.DatabaseFlavor;
using Saga.Orchestrator.Worker.Infra.Contexts;

namespace Saga.Orchestrator.Worker.Configuration
{
    public static class PostgresConfig
    {
        public static void AddPostgresConfigurationMassTransit<TSaga>(IEntityFrameworkSagaRepositoryConfigurator<TSaga> configure)
            where TSaga : class, ISaga
        {
            configure.AddDbContext<DbContext, FullExportStateDbContext>((provider, builder) =>
            {
                builder.UseNpgsql(m =>
                {
                    m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    m.MigrationsHistoryTable($"__{{nameof(OrderStateDbContext)}}");
                });
            });

            configure.UsePostgres();
        }

        public static void SetEntityFrameworkPostgresProvider(IEntityFrameworkSagaRepositoryConfigurator cfg)
        {
            cfg.ExistingDbContext<FullExportStateDbContext>();
            cfg.UsePostgres();
        }

        public static void AddPostgresConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            service.ConfigureProviderForContextPostgres<FullExportStateDbContext>(
                ProviderConfiguration.DetectDatabase(configuration));
        }
    }
}
