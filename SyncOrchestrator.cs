
using VDSReconciliation;

public class SyncOrchestrator : ISyncOrchestrator
{
    private readonly IDetailRepository _detailRepo;
    private readonly IUserAppAuthorityRepository _authorityRepo;
    private readonly IServiceBusPublisher _publisher;
    private readonly IMasterRepository _masterRepo;
    private static readonly TimeSpan interval = TimeSpan.FromSeconds(5); // Later fetch from keyvault or config file

    public SyncOrchestrator(
        IDetailRepository detailRepo,
        IUserAppAuthorityRepository authorityRepo,
        IServiceBusPublisher publisher,
        IMasterRepository masterRepo)
    {
        _detailRepo = detailRepo;
        _authorityRepo = authorityRepo;
        _publisher = publisher;
        _masterRepo = masterRepo;
    }

    public async Task ProcessAsync(SyncRequestMessage message)
    {
        if (message.IsFullSync)
            await HandleFullSyncAsync(message);
        else
            await HandleDeltaSyncAsync(message);
    }

    private async Task HandleDeltaSyncAsync(SyncRequestMessage message)
    {
        var details = await _detailRepo.GetDetailsByMasterIdAsync(message.Id);
        foreach (var detail in details)
        {
            try
            {
                var records = await _authorityRepo.GetByEidAndGlinAsync(detail.EID, detail.GLIN, detail.Application_ID);
                foreach (var record in records)
                    await _publisher.PublishAsync(record);

                await _detailRepo.UpdateStatusAsync(detail.ID, 3, null);
            }
            catch (Exception ex)
            {
                await _detailRepo.UpdateStatusAsync(detail.ID, 2, ex.Message);
            }
        }
        await UpdateMasterAsync(message.Id);
    }

    private async Task HandleFullSyncAsync(SyncRequestMessage message)
    {
        var records = await _authorityRepo.GetByApplicationIdAsync(message.Application_ID);
        var details = records.Select(r => new DetailRecord { EID = r.user_id, GLIN = r.location_id }).ToList();
        await _detailRepo.InsertBulkAsync(message.Id, details);

        TimeSpan baseDelay = TimeSpan.Zero;
        foreach (var detail in details)
        {
            try
            {
                var record = records.First(r => r.user_id == detail.EID && r.location_id == detail.GLIN);
                await _publisher.PublishAsync(record);

                ServiceBus.SendToServiceBusWithDelayAsync(
                                    AppObj: record,
                                    appName: record.application_id,
                                    operationName: "StoreAppAccessApplication - DeletedFutureStoreTransactions",
                                    provisionedType: "Delete",
                                    delay: baseDelay
                                ).GetAwaiter().GetResult();
                baseDelay = baseDelay.Add(interval);

                await _detailRepo.UpdateStatusAsync(detail.ID, 3, null);
            }
            catch (Exception ex)
            {
                await _detailRepo.UpdateStatusAsync(detail.ID, 2, ex.Message);
            }
        }
        await UpdateMasterAsync(message.Id);
    }

    private async Task UpdateMasterAsync(long batchID)
    {
        var summary = await _detailRepo.GetProcessingSummaryAsync(batchID);
        await _masterRepo.UpdateStatusAsync(batchID, summary.TotalRecords, summary.SuccessCount, summary.FailedCount);
    }
}
