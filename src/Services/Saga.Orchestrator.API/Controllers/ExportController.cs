using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.API.Models.Requests;
using Saga.Orchestrator.Core.DomainObjects;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Commands;
using Saga.Orchestrator.Core.Messages.IntegrationContracts.Events;

namespace Saga.Orchestrator.API.Controllers
{
    [Route("api/[Controller]")]
    public sealed class ExportController : SagaControllerBase
    {
        private readonly IRequestClient<ISubmitFullExport> _submitFullExport;
        private readonly IRequestClient<ICheckStatus> _checkFullExport;

        public ExportController(IRequestClient<ISubmitFullExport> submitFullExportRequestedEvent, IRequestClient<ICheckStatus> checkFullExport)
        {
            _submitFullExport = submitFullExportRequestedEvent;
            _checkFullExport = checkFullExport;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IFullExportStatus), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IFullExportNotFound), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var tasks = _checkFullExport.GetResponse<IFullExportStatus, IFullExportNotFound>(new
            {
                ExportId = id
            });

            tasks.Wait(10000);

            if (!tasks.IsCompleted) return StatusCode(StatusCodes.Status500InternalServerError);

            var (status, notFound) = await tasks;

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(IFullExportAccepted), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Export(ExportRequest model)
        {
            var task = _submitFullExport.GetResponse<IFullExportAccepted>(new
            {
                Cpf = new Cpf(model.Cpf)
            });

            task.Wait(11000);

            if (task.IsCompletedSuccessfully)
            {
                var response = await task;
                return Ok(response.Message);
            }

            if (task.IsCompleted)
            {
                var response = await task;
                return BadRequest(response.Message);
            }

            return BadRequest("Falha ao enviar requisição");
        }
    }
}
