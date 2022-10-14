namespace JoggingApp.OutboxPublishingAgent
{
    public enum EventType : int
    {
        DomainEvent = 1,
        IntegrationEvent = 2
    }
}
