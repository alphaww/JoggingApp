using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record JogLocationSetDomainEvent(Guid JogId, Coordinates Coordinates) : IDomainEvent
    {
        public DomainEventDispatchingStrategy DomainEventDispatchingStrategy => DomainEventDispatchingStrategy.EventualConsistency;
    }
}
