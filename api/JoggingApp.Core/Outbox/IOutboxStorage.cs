namespace JoggingApp.Core.Outbox
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetOutboxEventsAsync(int batchSize = 20);
        Task UpdateOutboxEventAsync(OutboxMessage outboxEvent);
        Task InsertOutboxEventAsync(OutboxMessage outboxEvent);
    }
}
