using JoggingApp.BuildingBlocks.EventBus.Events;
using MediatR;

namespace JoggingApp.Core
{
    public class Entity
    {
        private readonly List<DomainEventBase> _domainEvents = new();
        public Guid Id { get; }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity()
        {
        }

        public IReadOnlyCollection<DomainEventBase> GetDomainEvents() => _domainEvents.ToList();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(DomainEventBase domainEvent) =>
            _domainEvents.Add(domainEvent);
    }

    public abstract record IntegrationEventBase : IntegrationEvent, IEvent
    {
    }

    public abstract record DomainEventBase : IEvent
    {
        public virtual ConsistencyStrategy ConsistencyStrategy => 
            ConsistencyStrategy.StandardDispatch;
    }

    public interface IEvent : INotification
    {
    }

    public enum ConsistencyStrategy : int
    {
        StandardDispatch = 1,
        EventualConsistency = 2
    }

}
