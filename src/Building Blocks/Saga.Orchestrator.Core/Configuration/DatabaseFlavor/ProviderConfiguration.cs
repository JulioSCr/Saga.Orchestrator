using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Saga.Orchestrator.Core.Configuration.DatabaseFlavor
{
    public sealed class ProviderConfiguration
    {
        private readonly string _connectionString;
        public static ProviderConfiguration With;

        private static readonly string MigrationAssembly =
            typeof(ProviderConfiguration).GetTypeInfo().Assembly.GetName().Name;

        public ProviderConfiguration(string connectionString) => this._connectionString = connectionString;

        public static void Build(string cns)
        {
            if (ProviderConfiguration.With != null)
                return;
            ProviderConfiguration.With = new ProviderConfiguration(cns);
        }

        public Action<DbContextOptionsBuilder> Postgres
        {
            get
            {
                return (Action<DbContextOptionsBuilder>)(options => options.UseNpgsql(this._connectionString));
            }
        }

        public static string DetectDatabase(IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");
    }
}
