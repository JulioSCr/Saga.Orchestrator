using Saga.Orchestrator.API.Enums;
using Saga.Orchestrator.API.Extensions.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Saga.Orchestrator.API.Models.Requests
{
    public struct ExportRequest
    {
        [JsonPropertyName("cpf")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Cpf(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Cpf { get; init; }

        [JsonPropertyName("exportationType")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public ExportationType ExportationType { get; init; }
    }
}
