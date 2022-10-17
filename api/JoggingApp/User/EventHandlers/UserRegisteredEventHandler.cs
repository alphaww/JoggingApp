using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Users.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IEventBus _eventBus;
        public UserRegisteredEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
        {
            _eventBus.Publish(new SendEmailIntegrationEvent(@event.Email, "Test", "Test test 123 !!"));
            return Task.CompletedTask;
        }
    }
}
