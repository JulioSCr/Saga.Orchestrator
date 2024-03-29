﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Saga.Orchestrator.API.Models.Requests;
using Saga.Orchestrator.Core.Api.Controllers;
using Saga.Orchestrator.Core.DomainObjects;
using Saga.Orchestrator.Core.Messages.Integration.Commands;
using Saga.Orchestrator.Core.Messages.Integration.Contracts;
using Saga.Orchestrator.Core.Messages.Integration.Events;
using Saga.Orchestrator.Core.Messages.Integration.Responses;

namespace Saga.Orchestrator.API.Controllers
{
    [Route("api/[Controller]")]
    public sealed class ExportController : SagaControllerBase
    {
        private readonly IRequestClient<SubmitFullExportCommand> _submitFullExport;
        private readonly IRequestClient<ICheckStatus> _checkFullExport;

        public ExportController(IRequestClient<SubmitFullExportCommand> submitFullExportRequestedEvent, IRequestClient<ICheckStatus> checkFullExport)
        {
            _submitFullExport = submitFullExportRequestedEvent;
            _checkFullExport = checkFullExport;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IFullExportStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> GetStatus(Guid id)
        {
            var tasks = _checkFullExport.GetResponse<IFullExportStatusResponse, CustomResponse>(new
            {
                ExportId = id
            });

            tasks.Wait(5000);

            if (!tasks.IsCompleted) return StatusCode(StatusCodes.Status504GatewayTimeout);

            var (status, notFound) = await tasks;

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return CustomResponse(response.Message);
            }
        }

        [HttpPost()]
        [ProducesResponseType(typeof(FullExportAcceptedEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> Export(ExportRequest model)
        {
            var tasks = _submitFullExport.GetResponse<FullExportAcceptedEvent, CustomResponse>(new
            {
                Cpf = new Cpf(model.Cpf),
                Timestamp = DateTime.UtcNow,
            });

            tasks.Wait(5000);

            if (!tasks.IsCompleted) return StatusCode(StatusCodes.Status504GatewayTimeout);

            var (accepted, rejected) = await tasks;

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response.Message);
            }

            if (rejected.IsCompletedSuccessfully)
            {
                var response = await rejected;
                AddProcessError(response.Message.Errors);
                return CustomResponse();
            }

            AddProcessError("Ocorreu um erro não esperado");
            return CustomResponse();
        }
    }
}
