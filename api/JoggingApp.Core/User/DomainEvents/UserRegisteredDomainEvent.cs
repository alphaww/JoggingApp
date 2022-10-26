namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(string Email) : DomainEventBase
    {
    }
}
