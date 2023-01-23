using Dapper;
using JoggingApp.Core.Outbox;
using System.Data;

namespace JoggingApp.OutboxPublishingAgent
{
    public class OutboxStorage : IOutboxStorage
    {
        private readonly IDbConnection _connection;

        public OutboxStorage(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<OutboxMessage>> MarkAndGetOutboxEvents()
        {
            var now = DateTime.UtcNow;
            var sql = @"UPDATE OutboxMessage SET EventState = @pendingState
                        OUTPUT deleted.*
                        WHERE EventState = @readyState AND ProcessedOnUtc IS NULL AND OccurredOnUtc <= @now";

            var outboxEvents = await _connection.QueryAsync<OutboxMessage>(sql,
                new { now, pendingState = OutboxMessageState.Pending, readyState = OutboxMessageState.Ready });

            return outboxEvents.ToList();
        }

        public async Task UpdateOutboxEventAsync(OutboxMessage outboxEvent)
        {
            var sql = @"UPDATE OutboxMessage SET EventState = @EventState, ProcessedOnUtc = @ProcessedOnUtc, Error = @Error 
                WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, outboxEvent);

        }

        public async Task InsertOutboxEventAsync(OutboxMessage outboxEvent)
        {
            var sql = @"INSERT INTO OutboxMessage (Id, Type, Content, EventType, EventState, OccurredOnUtc, ProcessedOnUtc, Error)
                VALUES (@Id, @Type, @Content, @EventType, @EventState, @OccurredOnUtc, @ProcessedOnUtc, @Error)";

            await _connection.ExecuteAsync(sql, outboxEvent);
        }
    }
}
