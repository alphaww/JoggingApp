namespace JoggingApp.Core.Outbox
{
    public enum OutboxMessageState : int
    {
        Ready = 1,
        Pending = 2,
        Done = 3,
        Fail = 4
    }
}
