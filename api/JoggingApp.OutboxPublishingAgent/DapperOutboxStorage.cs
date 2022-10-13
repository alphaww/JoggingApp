using Dapper;
using JoggingApp.Core.Outbox;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace JoggingApp.OutboxPublishingAgent
{
    public class DapperOutboxStorage : IOutboxStorage
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperOutboxStorage(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DefaultConnection"];
        }

        public async Task<IEnumerable<OutboxMessage>> GetOutboxEvents()
        {
            const string sql = "select [Id], [Type], [Content] " +
                               "from [dbo].[OutboxMessage] " +
                               "where [EventState] = 1 OR [EventState] = 3 ";

            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var changes = await connection.QueryAsync<OutboxMessage>(sql).ConfigureAwait(false);
            return changes;
        }

        public async Task UpdateOutboxEventStateToProcessed(Guid Id)
        {
            const string sql = "update [dbo].[OutboxMessage] " +
                               "set EventState = 2, ProcessedOnUtc = @now " +
                               "where [Id] = @Id;";

            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            await connection.ExecuteAsync(sql, new { Id, @now = DateTime.UtcNow }).ConfigureAwait(false);
        }

        public async Task UpdateOutboxEventStateToFailed(Guid Id, string error)
        { 
            string sql = $"update [dbo].[OutboxMessage] set EventState = 3, ProcessedOnUtc = @now, Error = @error" +
                         $" where [Id] = @Id;";

            await using var connection = new SqlConnection(_connectionString);
            connection.Open();
            await connection.ExecuteAsync(sql, new { Id, @error = error, @now = DateTime.UtcNow }).ConfigureAwait(false);
        }
    }
}
