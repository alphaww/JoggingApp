using JoggingApp.Core;
using JoggingApp.Core.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace JoggingApp.EntityFramework.Interceptors
{
    public class DomainEventsHandlerInterceptor : SaveChangesInterceptor
    {
        private DbContext _dbContext;
        private readonly IPublisher _publisher;

        public DomainEventsHandlerInterceptor(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            _dbContext = eventData.Context;

            if (_dbContext is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var domainEvents = _dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();


            ProcessStandardDispatchEvents(domainEvents.Where(de =>
                    de.DomainEventDispatchingStrategy == DomainEventDispatchingStrategy.StandardDispatch)); 

            ProcessOutboxEvents(domainEvents.Where(de =>
                    de.DomainEventDispatchingStrategy == DomainEventDispatchingStrategy.EventualConsistency));

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ProcessOutboxEvents(IEnumerable<IDomainEvent> events)
        {
            var outboxMessages = events
                .Select(domainEvent =>
                {
                    var content = JsonConvert.SerializeObject(
                        domainEvent,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                    return new OutboxMessage(domainEvent.GetType().Namespace, content, EventType.DomainEvent,
                        DateTime.UtcNow);

                })
                .ToList();

            _dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        }

        private void ProcessStandardDispatchEvents(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                _publisher.Publish(domainEvent);
            }
        }
    }
}
