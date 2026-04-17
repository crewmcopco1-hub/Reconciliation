
public interface IUserAppAuthorityRepository
{
    Task<List<UserAppAuthority>> GetByEidAndGlinAsync(string eid, string glin, string Application_ID);
    Task<List<UserAppAuthority>> GetByApplicationIdAsync(string Application_ID);
}
