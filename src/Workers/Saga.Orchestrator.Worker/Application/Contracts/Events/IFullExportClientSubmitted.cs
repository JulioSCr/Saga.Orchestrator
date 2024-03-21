﻿using Saga.Orchestrator.Core.DomainObjects;

namespace Saga.Orchestrator.Worker.Application.Contracts.Events
{
    public interface IFullExportClientSubmitted
    {
        Guid ExportId { get; }
        Cpf Cpf { get; }
        DateTime Timestamp { get; }
    }
}
