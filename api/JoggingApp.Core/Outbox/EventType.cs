namespace JoggingApp.Core.Outbox
{
    public enum EventType : int
    {
        DomainEvent = 1,
        IntegrationEvent = 2
    }
}
