using JoggingApp.Core;
using JoggingApp.Core.Outbox;
using JoggingApp.OutboxPublishingAgent;
using MediatR;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;

namespace JoggingApp.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class BackgroundPublishingService : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly IPublisher _publisher;
        private readonly OutboxStorage _outboxStore;
        public BackgroundPublishingService(IPublisher publisher)
        {
            _publisher = publisher;
            _outboxStore = new OutboxStorage("");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _outboxStore.GetOutboxEvents();

            foreach (OutboxMessage outboxMessage in messages)
            {
                IDomainEvent domainEvent = JsonConvert
                    .DeserializeObject<IDomainEvent>(
                        outboxMessage.Content,
                        JsonSerializerSettings);

                if (domainEvent is null)
                {
                    continue;
                }

                AsyncRetryPolicy policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(
                        3,
                        attempt => TimeSpan.FromMilliseconds(50 * attempt));

                PolicyResult result = await policy.ExecuteAndCaptureAsync(() =>
                    _publisher.Publish(
                        domainEvent,
                        context.CancellationToken));

                if (result.FinalException is not null)
                {
                   await _outboxStore.UpdateOutboxEventStateToFailed(outboxMessage.Id);
                }
                else
                {
                    await _outboxStore.UpdateOutboxEventStateToProcessed(outboxMessage.Id);
                }
            }
        }
    }
}
