using Dapper;
using JoggingApp.Core.Outbox;
using Microsoft.Data.SqlClient;

namespace JoggingApp.OutboxPublishingAgent
{
    public class OutboxStorage
    {
        private string _connectionString;

        public OutboxStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<OutboxMessage>> GetOutboxEvents()
        {
            const string sql = "select [Id], [MessageType], [Message] " +
                               "from [dbo].[OutboxMessage] " +
                               "where [EventState] = 1 " +
                               "order by OccurredOnUtc asc;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var changes = await connection.QueryAsync<OutboxMessage>(sql).ConfigureAwait(false);
                return changes;
            }
        }

        public async Task UpdateOutboxEventStateToProcessed(Guid Id)
        {
            const string sql = "update [dbo].[OutboxMessage] " +
                               "set EventState = 2, ProcessedOnUtc = @now " +
                               "where [Id] = @Id and [EventState] = 1;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Id, @now = DateTime.UtcNow }).ConfigureAwait(false);
            }
        }

        public async Task UpdateOutboxEventStateToFailed(Guid Id)
        {
            const string sql = "update [dbo].[OutboxMessage] " +
                               "set EventState = 3, ProcessedOnUtc = @now " +
                               "where [Id] = @Id and [EventState] = 1;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Id, @now = DateTime.UtcNow }).ConfigureAwait(false);
            }
        }
    }
}
