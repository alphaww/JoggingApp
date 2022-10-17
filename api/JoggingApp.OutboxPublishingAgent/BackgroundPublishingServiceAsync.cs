using JoggingApp.Core;
using JoggingApp.Core.Outbox;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;

namespace JoggingApp.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class BackgroundPublishingServiceAsync : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundPublishingServiceAsync(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();

            var outboxEvents = await repository.GetOutboxEventsAsync();
            
            foreach (var @event in outboxEvents)
            {
                @event.SetEventState(OutboxMessageState.InTransit);
                await repository.UpdateOutboxEventAsync(@event);

                HandleEvent(@event);
            }
        }

        public void HandleEvent(OutboxMessage @event)
        {
            Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();

                var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
                var repository = scope.ServiceProvider.GetRequiredService<IOutboxStorage>();

                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(@event.Content, JsonSerializerSettings);

                try
                {
                    await publisher.Publish(domainEvent);

                    @event.SetEventState(OutboxMessageState.Done);
                    await repository.UpdateOutboxEventAsync(@event);
                }
                catch 
                {
                    @event.SetEventState(OutboxMessageState.Fail);
                    await repository.UpdateOutboxEventAsync(@event);
                }
            });
        }
    }
}