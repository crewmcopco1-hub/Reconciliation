
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class ReconciliationFunction
{
    private readonly ISyncOrchestrator _orchestrator;
    private readonly ILogger<ReconciliationFunction> _logger;

    public ReconciliationFunction(ISyncOrchestrator orchestrator, ILogger<ReconciliationFunction> logger)
    {
        _orchestrator = orchestrator;
        _logger = logger;
    }

    [Function("ReconciliationFunction")]
    public async Task RunAsync(
    [ServiceBusTrigger(
        topicName: "topicintegrationevents",
        subscriptionName: "reconciliation-subscription",
        Connection = "ServiceBusConnection")]
    SyncRequestMessage message)
    {
        _logger.LogInformation(
            "Processing reconciliation message. Id={Id}, IsFullSync={IsFullSync}",
            message.Id,
            message.IsFullSync);

        await _orchestrator.ProcessAsync(message);
    }

}
