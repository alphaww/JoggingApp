﻿using JoggingApp.BuildingBlocks.EventBus.Events;
using MediatR;

namespace JoggingApp.Core
{
    public class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public Guid Id { get; }

        protected Entity(Guid id)
        {
            Id = id;
        }

        protected Entity()
        {
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

        public void ClearDomainEvents() => _domainEvents.Clear();

        protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
            _domainEvents.Add(domainEvent);
    }

    public record IntegrationEventBase : IntegrationEvent, IEvent
    {
    }

    public interface IDomainEvent : IEvent
    {
        public DomainEventConsistencyStrategy EventConsistencyStrategy { get; }
    }

    public interface IEvent : INotification
    {
    }

    public enum DomainEventConsistencyStrategy : int
    {
        StandardDispatch = 1,
        EventualConsistency = 2
    }

}
