namespace JoggingApp.Core.Outbox
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetUnprocessedOutboxEvents();

        Task UpdateOutboxEventStateToProcessed(Guid Id);

        Task UpdateOutboxEventStateToFailed(Guid Id, string error);

    }
}
