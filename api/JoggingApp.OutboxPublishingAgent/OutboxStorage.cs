using JoggingApp.Core.Outbox;
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

        public async Task<IEnumerable<OutboxMessage>> GetOutboxEventsAsync(int batchSize = 10)
        {
            return await _queryFactory.Query(nameof(OutboxMessage))
                .Select(nameof(OutboxMessage.Id), nameof(OutboxMessage.Type), nameof(OutboxMessage.Content),
                    nameof(OutboxMessage.OccurredOnUtc), nameof(OutboxMessage.ProcessedOnUtc), nameof(OutboxMessage.Error))
                .WhereIn(nameof(OutboxMessage.EventState), new[] { OutboxMessageState.ReadyForProcessing , OutboxMessageState.Failed })
                .OrderBy(nameof(OutboxMessage.OccurredOnUtc))
                .Take(batchSize)
                .GetAsync<OutboxMessage>();
        }

        public async Task UpdateOutboxEventsAsync(IEnumerable<OutboxMessage> outboxEvents)
        {
            if (!outboxEvents.Any())
                await Task.CompletedTask;

            var query = new Query(nameof(OutboxMessage));
            foreach (var @event in outboxEvents)
            {
                query = query
                    .Where(nameof(OutboxMessage.Id), @event.Id)
                    .AsUpdate(new
                    {
                        @event.Type,
                        @event.Content,
                        @event.EventState,
                        @event.OccurredOnUtc,
                        @event.ProcessedOnUtc,
                        @event.Error
                    });
            }
            await _queryFactory.ExecuteAsync(query);
        }
    }
}
