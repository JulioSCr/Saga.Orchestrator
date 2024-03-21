using FluentValidation.Results;

namespace Saga.Orchestrator.Core.Messages
{
    public abstract class Command : Message
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
