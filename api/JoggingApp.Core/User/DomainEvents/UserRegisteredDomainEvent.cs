namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(string Email) : IDomainEvent
    {
        public DomainEventDispatchingStrategy DomainEventDispatchingStrategy => DomainEventDispatchingStrategy.EventualConsistency;
    }
}
