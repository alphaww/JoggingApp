using Dapper;
using JoggingApp.Core.Outbox;
using Microsoft.Data.SqlClient;
using SqlKata;
using SqlKata.Execution;

namespace JoggingApp.OutboxPublishingAgent
{
    public class OutboxStorage : IOutboxStorage
    {
        private readonly QueryFactory _queryFactory;

        public OutboxStorage(QueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public async Task<IEnumerable<OutboxMessage>> MarkAsTransitAndFetchReadyOutboxEventsAsync(int batchSize = 10)
        {
            return await _queryFactory.Query(nameof(OutboxMessage))
                .Select(nameof(OutboxMessage.Id), nameof(OutboxMessage.Type), nameof(OutboxMessage.Content),
                    nameof(OutboxMessage.OccurredOnUtc), nameof(OutboxMessage.ProcessedOnUtc), nameof(OutboxMessage.Error))
                .WhereIn(nameof(OutboxMessage.EventState), new[] { OutboxMessageState.Ready , OutboxMessageState.Fail })
                .OrderBy(nameof(OutboxMessage.OccurredOnUtc))
                .Take(batchSize)
                .GetAsync<OutboxMessage>();
        }

        public async Task UpdateOutboxEventAsync(OutboxMessage outboxEvent)
        {
            await _queryFactory
                .Query(nameof(OutboxMessage))
                .Where(nameof(OutboxMessage.Id), outboxEvent.Id)
                .UpdateAsync(new
                {
                    outboxEvent.Type,
                    outboxEvent.Content,
                    outboxEvent.EventState,
                    outboxEvent.OccurredOnUtc,
                    outboxEvent.ProcessedOnUtc,
                    outboxEvent.Error
                });
        }

        public async Task InsertOutboxEventAsync(OutboxMessage outboxEvent)
        {
            await _queryFactory
                .Query(nameof(OutboxMessage))
                .InsertAsync(new
                {
                    outboxEvent.Id,
                    outboxEvent.Type,
                    outboxEvent.Content,
                    outboxEvent.EventState,
                    outboxEvent.OccurredOnUtc,
                    outboxEvent.ProcessedOnUtc,
                    outboxEvent.Error
                });
        }
    }
}
