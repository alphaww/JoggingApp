using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record UserRegisteredDomainEvent(Guid UserId, User User) : IDomainEvent
    {
    }
}
