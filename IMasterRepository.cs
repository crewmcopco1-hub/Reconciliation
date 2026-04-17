
public interface IMasterRepository
{
    Task UpdateStatusAsync(long masterId, int total, int success, int failed);
}
