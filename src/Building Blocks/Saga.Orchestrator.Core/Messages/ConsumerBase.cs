using MassTransit;
using Saga.Orchestrator.Core.Messages.Integration.Responses;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Saga.Orchestrator.Core.Messages
{
    public abstract class ConsumerBase
    {
        protected IList<string> Errors { get; private set; } = new List<string>();

        protected void AddError(string message)
        {
            Errors.Add(message);
        }

        protected void AddError(ValidationResult validatoResult)
        {
            validatoResult.Errors.ForEach(e => Errors.Add(e.ErrorMessage));
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected async Task CustomResponseAsync<TContext>(ConsumeContext<TContext> context)
            where TContext : Command
        {
            await context.RespondAsync<CustomResponse>(new CustomResponse
            {
                Timestamp = InVar.Timestamp,
                Errors = Errors
            });
        }
    }
}
