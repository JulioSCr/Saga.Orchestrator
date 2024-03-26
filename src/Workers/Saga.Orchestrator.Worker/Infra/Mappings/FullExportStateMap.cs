//using MassTransit;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Saga.Orchestrator.Worker.Application.StateMachines;

//namespace Saga.Orchestrator.Worker.Infra.Mappings
//{
//    public sealed class FullExportStateMap : SagaClassMap<FullExportState>
//    {
//        protected override void Configure(EntityTypeBuilder<FullExportState> entity, ModelBuilder model)
//        {
//            entity.Property(x => x.CurrentState).HasMaxLength(64);
//            entity.Property(x => x.SubmitDate);
//            entity.Property(x => x.Cpf);

//            // If using Optimistic concurrency, otherwise remove this property
//            entity.Property(x => x.Version)
//                .HasColumnName("xmin")
//                .HasColumnType("xid")
//                .IsRowVersion();
//        }
//    }
//}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saga.Orchestrator.Worker.Application.StateMachines;

namespace Saga.Orchestrator.Worker.Infra.Mappings
{
    public sealed class FullExportStateMap : IEntityTypeConfiguration<FullExportState>
    {
        public void Configure(EntityTypeBuilder<FullExportState> builder)
        {
            builder.HasKey(c => c.CorrelationId);

            builder.Property(c => c.CorrelationId)
                .ValueGeneratedNever()
                .HasColumnName("ExportId");

            builder.Property(c => c.Cpf);
            builder.Property(c => c.Updated);
            builder.Property(c => c.SubmitDate);
            builder.Property(c => c.CurrentState);
        }
    }
}
