using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record JogLocationSetDomainEvent(Guid JogId, Coordinates Coordinates) : IDomainEvent
    {
        public DomainEventConsistencyStrategy DomainEventConsistencyStrategy => DomainEventConsistencyStrategy.EventualConsistency;
    }
}
