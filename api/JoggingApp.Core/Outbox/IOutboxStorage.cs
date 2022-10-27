namespace JoggingApp.Core.Outbox
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> MarkAndGetOutboxEvents(int batchSize = 20);
        Task UpdateOutboxEventAsync(OutboxMessage outboxEvent);
        Task InsertOutboxEventAsync(OutboxMessage outboxEvent);
    }
}
