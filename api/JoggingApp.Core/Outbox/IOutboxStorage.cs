﻿namespace JoggingApp.Core.Outbox
{
    public interface IOutboxStorage
    {
        Task<IEnumerable<OutboxMessage>> GetOutboxEventsAsync(int batchSize = 20);

        Task UpdateOutboxEventAsync(OutboxMessage outboxEvent);

        Task UpdateOutboxEventsAsync(IEnumerable<OutboxMessage> outboxEvents);

    }
}
