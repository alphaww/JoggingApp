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

        public async Task<IEnumerable<OutboxMessage>> GetOutboxEventsAsync(int batchSize = 10)
        {
            return await _queryFactory.Query(nameof(OutboxMessage))
                .Select(nameof(OutboxMessage.Id), nameof(OutboxMessage.Type), nameof(OutboxMessage.Content),
                    nameof(OutboxMessage.OccurredOnUtc), nameof(OutboxMessage.ProcessedOnUtc), nameof(OutboxMessage.Error))
                .WhereIn(nameof(OutboxMessage.EventState), new[] { OutboxMessageState.ReadyForProcessing , OutboxMessageState.Fail })
                .OrderBy(nameof(OutboxMessage.OccurredOnUtc))
                .Take(batchSize)
                .GetAsync<OutboxMessage>();
        }

        public async Task UpdateOutboxEventAsync(OutboxMessage outboxEvent)
        {
            await UpdateOutboxEventsAsync(new[] { outboxEvent });
        }

        public async Task UpdateOutboxEventsAsync(IEnumerable<OutboxMessage> outboxEvents)
        {
            if (outboxEvents.Any())
            {
                foreach (var @event in outboxEvents)
                {

                    //This is bad.. needs to go into transaction ( sql )
                    await _queryFactory
                        .Query(nameof(OutboxMessage))
                        .Where(nameof(OutboxMessage.Id), @event.Id)
                        .UpdateAsync(new
                        {
                            @event.Type,
                            @event.Content,
                            @event.EventState,
                            @event.OccurredOnUtc,
                            @event.ProcessedOnUtc,
                            @event.Error
                        });
                }

                //await _queryFactory.ExecuteAsync(query);
            }
        }
    }
}
