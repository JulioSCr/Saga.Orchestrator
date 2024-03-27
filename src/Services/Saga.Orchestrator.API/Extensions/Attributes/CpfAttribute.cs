using Saga.Orchestrator.Core.DomainObjects;
using System.ComponentModel.DataAnnotations;

namespace Saga.Orchestrator.API.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class CpfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var errorMessage = string.Format(ErrorMessageString, validationContext?.DisplayName ?? "");

            var cpf = value?.ToString();
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return new ValidationResult(errorMessage, new[] { validationContext?.MemberName ?? "" });
            }

            return Cpf.Validate(cpf) ? ValidationResult.Success : new ValidationResult(errorMessage, new[] { validationContext?.MemberName ?? "" });
        }
    }
}
