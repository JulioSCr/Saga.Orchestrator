using MassTransit;
using Saga.Orchestrator.Core.Messages.Integration.Contracts;
using Saga.Orchestrator.Core.Messages.Integration.Responses;
using Saga.Orchestrator.Worker.Application.Activities;
using Saga.Orchestrator.Worker.Application.Contracts;

namespace Saga.Orchestrator.Worker.Application.StateMachines
{
    public sealed class FullExportStateMachine :
        MassTransitStateMachine<FullExportState>
    {
        public FullExportStateMachine()
        {
            Event(() => FullExportClientSubmittedEvent, x => x.CorrelateById(m => m.Message.ExportId));
            Event(() => BasicDataSentEvent, x => x.CorrelateById(m => m.Message.ExportId));
            Event(() => FullExportStatusRequested, x =>
            {
                x.CorrelateById(m => m.Message.ExportId);
                x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    if (context.RequestId.HasValue)
                    {
                        await context.RespondAsync<CustomResponse>(
                            new CustomResponse(
                                InVar.Timestamp,
                                "Não foi possível identificar um processo neste ID."));
                    }
                }));
                x.ReadOnly = true;
            });

            InstanceState(x => x.CurrentState);

            Initially(
                When(FullExportClientSubmittedEvent)
                    .Then(context =>
                    {
                        context.Saga.SubmitDate = context.Message.Timestamp;
                        context.Saga.Cpf = context.Message.Cpf.Value;
                        context.Saga.Updated = DateTime.UtcNow;
                    })
                    .TransitionTo(Submitted));

            During(BasicDataSent,
                When(BasicDataSentEvent)
                    .Activity(x => x.OfType<SendComplementsActivity>())
                    .TransitionTo(Completed));

            DuringAny(
                When(FullExportClientSubmittedEvent)
                    .Then(context =>
                    {
                        context.Saga.SubmitDate ??= context.Message.Timestamp;
                        context.Saga.Cpf ??= context.Message.Cpf.Value;
                    }),
                When(FullExportStatusRequested)
                    .RespondAsync(x => x.Init<IFullExportStatusResponse>(new
                    {
                        ExportId = x.Saga.CorrelationId,
                        State = x.Saga.CurrentState
                    }))
            );
        }

        public State Submitted { get; private set; } = null!;
        public State BasicDataSent { get; private set; } = null!;
        public State ComplementsSent { get; private set; } = null!;
        public State Completed { get; private set; } = null!;

        public Event<IFullExportClientSubmitted> FullExportClientSubmittedEvent { get; private set; } = null!;
        public Event<IBasicDataSent> BasicDataSentEvent { get; private set; } = null!;
        public Event<ICheckStatus> FullExportStatusRequested { get; private set; } = null!;
    }
}
