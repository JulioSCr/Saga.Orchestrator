using MassTransit;
using Saga.Orchestrator.Worker.Application.Contracts;
using Saga.Orchestrator.Worker.Application.StateMachines;

namespace Saga.Orchestrator.Worker.Application.Activities
{
    public sealed class SendComplementsActivity :
        IStateMachineActivity<FullExportState, IBasicDataSent>
    {
        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<FullExportState, IBasicDataSent> context, IBehavior<FullExportState, IBasicDataSent> next)
        {
            Console.WriteLine($"Sending Complements CPF {context.Message.Cpf}");
            await Task.Delay(20000);
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<FullExportState, IBasicDataSent, TException> context, IBehavior<FullExportState, IBasicDataSent> next) where TException : Exception
        {
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("send-basic-data");
        }
    }
}
