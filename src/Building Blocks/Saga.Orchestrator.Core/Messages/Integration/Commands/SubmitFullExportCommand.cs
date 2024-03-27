using FluentValidation;
using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Core.Messages.Integration.Commands
{
    public sealed class SubmitFullExportCommand : Command
    {
        public Cpf Cpf { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SubmitFullImportCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SubmitFullImportCommandValidation : AbstractValidator<SubmitFullExportCommand>
    {
        public SubmitFullImportCommandValidation()
        {
            RuleFor(c => c.Cpf)
                .Must(cpf => Cpf.Validate(cpf.Value))
                .WithMessage("Cpf em formato inválido");
        }
    }
}
