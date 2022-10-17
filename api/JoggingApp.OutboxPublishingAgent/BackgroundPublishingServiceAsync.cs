using JoggingApp.Core;
using JoggingApp.Core.Outbox;
using JoggingApp.OutboxPublishingAgent;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
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

        private readonly IOutboxStorage _outboxStorage;
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundPublishingServiceAsync(IOutboxStorage outboxStorage, IServiceScopeFactory scopeFactory)
        {
            _outboxStorage = outboxStorage;
            _scopeFactory = scopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var outboxEvents = await _outboxStorage.GetOutboxEventsAsync();
            
            foreach (var @event in outboxEvents)
            {
                @event.SetEventState(OutboxMessageState.InTransit);
                await _outboxStorage.UpdateOutboxEventAsync(@event);

                Execute(@event);
            }
        }

        public void Execute(OutboxMessage @event)
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