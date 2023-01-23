using JoggingApp.Core.Outbox;
using SqlKata.Execution;
using System.Data;
using System.Data.Common;
using Dapper;

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
            var sql = @"UPDATE outbox SET ProcessStartedAt = @now, EventState = @pendingState 
                WHERE EventState = @readyState AND ProcessStartedAt IS NULL 
                AND OccurredOnUtc <= @now 
                OUTPUT inserted.*";

            using var transaction = _connection.BeginTransaction();
            var outboxEvents = await _connection.QueryAsync<OutboxMessage>(sql,
                new { now, pendingState = OutboxMessageState.Pending, readyState = OutboxMessageState.Ready },
                transaction);

            transaction.Commit();

            return outboxEvents.ToList();
        }

        public async Task UpdateOutboxEventAsync(OutboxMessage outboxEvent)
        {
            var sql = @"UPDATE outbox SET EventState = @EventState, ProcessedOnUtc = @ProcessedOnUtc, Error = @Error 
                WHERE Id = @Id";

            await _connection.ExecuteAsync(sql, outboxEvent);
        }

        public async Task InsertOutboxEventAsync(OutboxMessage outboxEvent)
        {
            var sql = @"INSERT INTO outbox (Id, Type, Content, EventType, EventState, OccurredOnUtc, ProcessedOnUtc, Error)
                VALUES (@Id, @Type, @Content, @EventType, @EventState, @OccurredOnUtc, @ProcessedOnUtc, @Error)";

            await _connection.ExecuteAsync(sql, outboxEvent);
        }
    }
}
