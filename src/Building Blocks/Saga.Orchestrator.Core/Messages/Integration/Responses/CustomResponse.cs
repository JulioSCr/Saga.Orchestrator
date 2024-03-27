namespace Saga.Orchestrator.Core.Messages.Integration.Responses
{
    public class CustomResponse : Message
    {
        public DateTime Timestamp { get; set; }
        public IList<string>? Errors { get; set; }

        public CustomResponse() { }

        public CustomResponse(DateTime timestamp, string message)
        {
            Timestamp = timestamp;
            Errors = new List<string> { message };
        }

        public bool Success => Errors == null || !Errors.Any();
    }
}
