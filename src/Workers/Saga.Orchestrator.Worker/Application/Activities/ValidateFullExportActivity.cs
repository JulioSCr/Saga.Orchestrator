using MassTransit;
using Saga.Orchestrator.Worker.Application.Contracts.Events;
using Saga.Orchestrator.Worker.Application.StateMachines;

namespace Saga.Orchestrator.Worker.Application.Activities
{
    public sealed class ValidateFullExportActivity :
        IStateMachineActivity<FullExportState, IFullExportClientSubmitted>
    {
        private readonly ILogger<ValidateFullExportActivity> _logger;

        public ValidateFullExportActivity(ILogger<ValidateFullExportActivity> logger)
        {
            _logger = logger;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("send-basic-data");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<FullExportState, IFullExportClientSubmitted> context, IBehavior<FullExportState, IFullExportClientSubmitted> next)
        {
            _logger.LogInformation($"Validate Full Export | Export Id ==> {context.Message.ExportId} | Cpf ==> {context.Message.Cpf}");



            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<FullExportState, IFullExportClientSubmitted, TException> context, IBehavior<FullExportState, IFullExportClientSubmitted> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }
}
