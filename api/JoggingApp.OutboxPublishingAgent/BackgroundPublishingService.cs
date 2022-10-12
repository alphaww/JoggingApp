﻿using JoggingApp.Core;
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
        private readonly OutboxStorage _outboxStorage;
        public BackgroundPublishingService(IPublisher publisher, OutboxStorage outboxStorage)
        {
            _publisher = publisher;
            _outboxStorage = outboxStorage;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _outboxStorage.GetOutboxEvents();

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

                if (result.Outcome == OutcomeType.Failure)
                {
                    await _outboxStorage.UpdateOutboxEventStateToFailed(outboxMessage.Id, result.FinalException?.ToString());
                }
                else
                {
                    await _outboxStorage.UpdateOutboxEventStateToProcessed(outboxMessage.Id);
                }
            }
        }
    }
}
