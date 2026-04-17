
public interface ISyncOrchestrator
{
    Task ProcessAsync(SyncRequestMessage message);
}
