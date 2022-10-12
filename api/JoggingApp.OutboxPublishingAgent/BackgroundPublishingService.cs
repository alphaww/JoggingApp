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
        private readonly IOutboxStorage _dapperOutboxStorage;
        public BackgroundPublishingService(IPublisher publisher, IOutboxStorage dapperOutboxStorage)
        {
            _publisher = publisher;
            _dapperOutboxStorage = dapperOutboxStorage;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var messages = await _dapperOutboxStorage.GetUnprocessedOutboxEvents();

            foreach (OutboxMessage outboxMessage in messages)
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings);

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

                //TODO: Update all in one transaction in ACID way
                if (result.Outcome == OutcomeType.Failure)
                {
                    await _dapperOutboxStorage.UpdateOutboxEventStateToFailed(outboxMessage.Id, result.FinalException?.ToString());
                }
                else
                {
                    await _dapperOutboxStorage.UpdateOutboxEventStateToProcessed(outboxMessage.Id);
                }
            }
        }
    }
}


/* ============================== EF CORE VERSION ====================================
 namespace JoggingApp.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        private readonly JoggingAppDbContext _dbContext;
        private readonly IPublisher _publisher;

        public ProcessOutboxMessagesJob(JoggingAppDbContext dbContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            List<OutboxMessage> messages = await _dbContext
                .Set<OutboxMessage>()
                .Where(m => m.ProcessedOnUtc == null &&
                            m.Error == null)
                .Take(20)
                .ToListAsync(context.CancellationToken);

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

                outboxMessage.SetProcessed(result.FinalException?.ToString());
                _dbContext.Entry(outboxMessage).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
 * */