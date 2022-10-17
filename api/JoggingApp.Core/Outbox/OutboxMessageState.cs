namespace JoggingApp.Core.Outbox
{
    public enum OutboxMessageState : int
    {
        ReadyForProcessing = 1,
        InTransit = 2,
        Done = 3,
        Fail = 4
    }
}
