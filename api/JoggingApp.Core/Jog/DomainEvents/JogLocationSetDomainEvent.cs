using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record JogLocationSetDomainEvent(Guid JogId, Coordinates Coordinates) : DomainEventBase
    {
        public override ConsistencyStrategy ConsistencyStrategy => 
            ConsistencyStrategy.EventualConsistency;
    }
}
