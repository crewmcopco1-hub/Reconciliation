
public interface IDetailRepository
{
    Task<List<DetailRecord>> GetDetailsByMasterIdAsync(long batchID);
    Task InsertBulkAsync(long batchID, List<DetailRecord> records);
    Task UpdateStatusAsync(long ID, int status, string? errorMessage);
    Task<ProcessingSummary> GetProcessingSummaryAsync(long batchID);
}
