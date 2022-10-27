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

        public async Task<IEnumerable<OutboxMessage>> MarkAndGetOutboxEvents(int batchSize = 10)
        {
            const string sql = @"BEGIN TRANSACTION

            DECLARE @UpdatedIDs table (Id UNIQUEIDENTIFIER)
            UPDATE [OutboxMessage] SET [EventState] = 2 
            OUTPUT inserted.Id 
            INTO @UpdatedIDs
            WHERE [EventState] = 1 OR [EventState] = 4;

            SELECT * FROM [OutboxMessage] m 
            WHERE 
              EXISTS 
                (SELECT 1 FROM @UpdatedIDs m2 WHERE m.Id = m2.Id)

            COMMIT";

            return await _queryFactory.SelectAsync<OutboxMessage>(sql);
        }

        public async Task UpdateOutboxEventAsync(OutboxMessage outboxEvent)
        {
            var q = _queryFactory
                .Query(nameof(OutboxMessage))
                .Where(nameof(OutboxMessage.Id), outboxEvent.Id);

            var sqlText = _queryFactory.Compiler.Compile(q).Sql;

            await q.UpdateAsync(new
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
