using MassTransit.Courier.Contracts;
using MassTransit;

namespace Saga.Orchestrator.Worker.Application.BatchConsumers
{
    public class RoutingSlipBatchEventConsumer :
         IConsumer<Batch<RoutingSlipCompleted>>
    {
        readonly ILogger<RoutingSlipBatchEventConsumer> _logger;

        public RoutingSlipBatchEventConsumer(ILogger<RoutingSlipBatchEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Batch<RoutingSlipCompleted>> context)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.Log(LogLevel.Information, "Routing Slips Completed: {TrackingNumbers}",
                    string.Join(", ", context.Message.Select(x => x.Message.TrackingNumber)));
            }

            return Task.CompletedTask;
        }
    }
}
