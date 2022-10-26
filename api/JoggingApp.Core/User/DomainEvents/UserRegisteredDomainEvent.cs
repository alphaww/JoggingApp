namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(string Email) : IDomainEvent
    {
        public DomainEventConsistencyStrategy EventConsistencyStrategy => DomainEventConsistencyStrategy.EventualConsistency;
    }
}
