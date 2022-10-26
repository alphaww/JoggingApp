using JoggingApp.Core;
using JoggingApp.Core.Outbox;
using MediatR;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace JoggingApp.OutboxPublishingAgent
{
    [DisallowConcurrentExecution]
    public class BackgroundPublishingService : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IPublisher _publisher;
        private readonly IOutboxStorage _outboxStorage;
        public BackgroundPublishingService(IPublisher publisher, IOutboxStorage outboxStorage)
        {
            _publisher = publisher;
            _outboxStorage = outboxStorage;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var outboxEvents = await _outboxStorage.GetOutboxEventsAsync();

            foreach (OutboxMessage @event in outboxEvents)
            {
                DomainEventBase domainEvent = JsonConvert.DeserializeObject<DomainEventBase>(@event.Content, JsonSerializerSettings);

                if (domainEvent is null)
                {
                    continue;
                }

                AsyncRetryPolicy policy = Policy.Handle<Exception>().WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(50 * attempt));

                PolicyResult result = await policy.ExecuteAndCaptureAsync(() => _publisher.Publish(domainEvent, context.CancellationToken));


                @event.SetEventState(result.Outcome == OutcomeType.Successful ? OutboxMessageState.Done: OutboxMessageState.Fail);
                await _outboxStorage.UpdateOutboxEventAsync(@event);
            }
        }
    }
}
