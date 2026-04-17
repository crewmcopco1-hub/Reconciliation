
public interface IServiceBusPublisher
{
    Task PublishAuthorityMessageAsync(AuthorityMessage msg,string? topicName = null);
}
