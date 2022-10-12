using JoggingApp.Core;
using MediatR;

namespace JoggingApp
{
    public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : IDomainEvent
    {
    }
}
