using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Saga.Orchestrator.API.Models.Responses;

namespace Saga.Orchestrator.API.Controllers
{
    [ApiController]
    public abstract class SagaControllerBase : ControllerBase
    {
        protected ICollection<string> Errors = new List<string>();

        protected IActionResult CustomResponse(object? result = null)
        {
            if (OperacaoValida())
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
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AdicionarErroProcessamento(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected IActionResult CustomResponse(ResponseResult resposta)
        {
            ResponsePossuiErros(resposta);

            return CustomResponse();
        }

        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta is null || !resposta.Errors.Mensagens.Any()) return false;

            foreach (var mensagem in resposta.Errors.Mensagens)
            {
                AdicionarErroProcessamento(mensagem);
            }

            return true;
        }

        protected bool OperacaoValida()
        {
            return !Errors.Any();
        }

        protected void AdicionarErroProcessamento(string erro)
        {
            Errors.Add(erro);
        }

        protected void LimparErrosProcessamento()
        {
            Errors.Clear();
        }
    }
}
