using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Saga.Orchestrator.Core.Messages.Integration.Responses;

namespace Saga.Orchestrator.Core.Api.Controllers
{
    [ApiController]
    public abstract class SagaControllerBase : ControllerBase
    {
        protected ICollection<string> Errors = new List<string>();

        protected IActionResult CustomResponse(object? result = null)
        {
            if (ValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Errors.ToArray() }
            }));
        }

        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                AddProcessError(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddProcessError(error.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IActionResult CustomResponse(CustomResponse response)
        {
            foreach (var error in response.Errors)
            {
                AddProcessError(error);
            }

            return CustomResponse();
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected void AddProcessError(string error)
        {
            Errors.Add(error);
        }

        protected void AddProcessError(IList<string>? errors)
        {
            if (errors is null) return;
            foreach (var error in errors)
            {
                Errors.Add(error);
            }
        }

        protected void ClearErrors()
        {
            Errors.Clear();
        }
    }
}
