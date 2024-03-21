using MassTransit;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Events;
using Saga.Orchestrator.Worker.Application.Contracts.Events;

namespace Saga.Orchestrator.Worker.Application.Consumers
{
    public sealed class SubmitFullExportConsumer : IConsumer<ISubmitFullExport>
    {
        readonly ILogger<SubmitFullExportConsumer> _logger;

        public SubmitFullExportConsumer() { }

        public SubmitFullExportConsumer(ILogger<SubmitFullExportConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ISubmitFullExport> context)
        {
            _logger.LogInformation($"FullExportClientConsumer {context.Message.Cpf}");

            var exportId = Guid.NewGuid();
            await context.Publish<IFullExportClientSubmitted>(new
            {
                exportId,
                context.Message.Cpf,
                context.Message.Timestamp
            });

            if (context.RequestId is not null)
                await context.RespondAsync<IFullExportAccepted>(new
                {
                    exportId,
                    InVar.Timestamp,
                    context.Message.Cpf
                });
        }
    }
}
