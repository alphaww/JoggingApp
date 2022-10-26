using JoggingApp.Core.Users;

namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(User User) : DomainEventBase
    {
    }
}
