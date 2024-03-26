using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Saga.Orchestrator.Worker.Application.StateMachines;
using Saga.Orchestrator.Worker.Infra.Mappings;

namespace Saga.Orchestrator.Worker.Infra.Contexts
{
    //public sealed class FullExportStateDbContext : SagaDbContext
    //{
    //    public FullExportStateDbContext(DbContextOptions options) : base(options) { }

    //    protected override IEnumerable<ISagaClassMap> Configurations { get { yield return new FullExportStateMap(); } }
    //}

    public sealed class FullExportStateDbContext : DbContext
    {
        public FullExportStateDbContext(DbContextOptions options) : base(options) { }

        public DbSet<FullExportState> FullExportStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FullExportStateMap());
        }
    }

}
