namespace JoggingApp.Core.Outbox
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> MarkAndGetOutboxEvents();
        Task UpdateOutboxEventAsync(OutboxMessage outboxEvent);
        Task InsertOutboxEventAsync(OutboxMessage outboxEvent);
    }
}
