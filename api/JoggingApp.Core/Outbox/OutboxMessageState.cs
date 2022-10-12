namespace JoggingApp.Core.Outbox
{
    public enum OutboxMessageState : int
    {
        ReadyForProcessing = 1,
        ProcessedSuccessfully = 2,
        Failed = 3
    }
}
