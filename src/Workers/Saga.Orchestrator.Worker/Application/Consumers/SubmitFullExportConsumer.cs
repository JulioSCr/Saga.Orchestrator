using MassTransit;
using Saga.Orchestrator.Core.DomainObjects;
using Saga.Orchestrator.Core.Messages;
using Saga.Orchestrator.Core.Messages.Integration.Commands;
using Saga.Orchestrator.Core.Messages.Integration.Events;
using Saga.Orchestrator.Worker.Application.Contracts;
using Saga.Orchestrator.Worker.Application.StateMachines;
using Saga.Orchestrator.Worker.Infra.Interfaces;

namespace Saga.Orchestrator.Worker.Application.Consumers
{
    public sealed class SubmitFullExportConsumer : ConsumerBase,
        IConsumer<SubmitFullExportCommand>
    {
        private readonly ILogger<SubmitFullExportConsumer> _logger;
        private readonly IFullExportStateRepository _stateRepository;

        public SubmitFullExportConsumer(ILogger<SubmitFullExportConsumer> logger, IFullExportStateRepository stateRepository)
        {
            _logger = logger;
            _stateRepository = stateRepository;
        }

        public async Task Consume(ConsumeContext<SubmitFullExportCommand> context)
        {
            _logger.LogInformation($"FullExportClientConsumer {context.Message.Cpf}");
            if (!context.Message.IsValid())
            {
                AddError(context.Message.ValidationResult);
                if (context.RequestId is not null) await CustomResponseAsync(context);
                return;
            }

            var activeExports = await _stateRepository.CountByCpfAndNotInState(context.Message.Cpf.Value,
                nameof(FullExportStateMachine.Completed));

            if (activeExports > 0)
            {
                AddError("Existem exportações em andamento para este cpf.");
                if (context.RequestId is not null) await CustomResponseAsync(context);
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
                await context.RespondAsync<FullExportAcceptedEvent>(new
                {
                    exportId,
                    InVar.Timestamp,
                    context.Message.Cpf
                });
        }
    }
}
