using System.Threading.Tasks;
using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.Users.IntegrationEvents;

namespace JoggingApp.Users.IntegrationEvents
{
    public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
    {
        public Task Handle(UserRegisteredIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
