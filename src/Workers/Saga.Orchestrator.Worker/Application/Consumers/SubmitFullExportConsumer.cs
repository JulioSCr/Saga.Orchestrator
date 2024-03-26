using MassTransit;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Events;
using Saga.Orchestrator.Worker.Application.Contracts.Events;
using Saga.Orchestrator.Worker.Application.StateMachines;
using Saga.Orchestrator.Worker.Infra.Interfaces;

namespace Saga.Orchestrator.Worker.Application.Consumers
{
    public sealed class SubmitFullExportConsumer : IConsumer<ISubmitFullExport>
    {
        private readonly ILogger<SubmitFullExportConsumer> _logger;
        private readonly IFullExportStateRepository _stateRepository;

        public SubmitFullExportConsumer() { }

        public SubmitFullExportConsumer(ILogger<SubmitFullExportConsumer> logger, IFullExportStateRepository stateRepository)
        {
            _logger = logger;
            _stateRepository = stateRepository;
        }

        public async Task Consume(ConsumeContext<ISubmitFullExport> context)
        {
            _logger.LogInformation($"FullExportClientConsumer {context.Message.Cpf}");

            var activeExports = await _stateRepository.CountByCpfAndNotInState(context.Message.Cpf.Value,
                nameof(FullExportStateMachine.Completed));

            if (activeExports > 0)
            {
                if (context.RequestId is not null)
                {
                    var reason = $"Existem {activeExports} exportações em andamento.";
                    await context.RespondAsync<IFullExportRejected>(new
                    {
                        InVar.Timestamp,
                        context.Message.Cpf,
                        reason
                    }); 
                }
                return;
            }

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
