using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.Users.IntegrationEvents;

namespace JoggingApp.EemailService
{
    public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public Task Handle(UserRegisteredIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
