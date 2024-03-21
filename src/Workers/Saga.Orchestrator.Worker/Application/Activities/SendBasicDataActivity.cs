using MassTransit;
using Saga.Orchestrator.Worker.Application.Contracts.Events;
using Saga.Orchestrator.Worker.Application.Events;
using Saga.Orchestrator.Worker.Application.StateMachines;

namespace Saga.Orchestrator.Worker.Application.Activities
{
    public sealed class SendBasicDataActivity :
        IStateMachineActivity<FullExportState, IFullExportClientSubmitted>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<FullExportState, IFullExportClientSubmitted> context, IBehavior<FullExportState, IFullExportClientSubmitted> next)
        {
            Console.WriteLine($"Sending Basic Data CPF {context.Message.Cpf}");

            await context.Publish<IBasicDataSent>(new
            {
                context.Message.ExportId,
                context.Message.Timestamp,
                context.Message.Cpf
            });

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<FullExportState, IFullExportClientSubmitted, TException> context, IBehavior<FullExportState, IFullExportClientSubmitted> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("send-basic-data");
        }
    }
}
