using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.IntegrationEvents;

namespace JoggingApp.EemailService
{
    public class SendEmailIntegrationEventHandler : IIntegrationEventHandler<SendEmailIntegrationEvent>
    {
        public Task Handle(SendEmailIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
