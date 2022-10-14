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

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            _dbContext = eventData.Context;

            if (_dbContext is null)
            {
               return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var domainEvents = _dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();
                    entity.ClearDomainEvents();
                    return domainEvents;
                })
                .ToList();

            await ProcessStandardDispatchEvents(domainEvents.Where(de =>
                    de.DomainEventDispatchingStrategy == DomainEventDispatchingStrategy.StandardDispatch),
                cancellationToken); 

           await ProcessOutboxDispatchEvents(domainEvents.Where(de =>
                    de.DomainEventDispatchingStrategy == DomainEventDispatchingStrategy.EventualConsistency),
                cancellationToken);

           return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task ProcessOutboxDispatchEvents(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
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

            await _dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
        }

        private async Task ProcessStandardDispatchEvents(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
        {
            foreach (var domainEvent in events)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }
    }
}
