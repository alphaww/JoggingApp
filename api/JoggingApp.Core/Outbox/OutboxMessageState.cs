namespace JoggingApp.Core.Outbox
{
    public enum OutboxMessageState : int
    {
        Ready = 1,
        Transit = 2,
        Done = 3,
        Fail = 4
    }
}
