namespace JoggingApp.Core.Outbox
{
    public enum OutboxMessageState : int
    {
        ReadyForProcessing = 1,
        Processed = 2,
        Failed = 3
    }
}
