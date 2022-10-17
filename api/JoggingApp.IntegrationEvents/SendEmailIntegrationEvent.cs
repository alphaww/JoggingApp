using JoggingApp.BuildingBlocks.EventBus.Events;

namespace JoggingApp.IntegrationEvents;

public record SendEmailIntegrationEvent(string Email, string Subject, string Body) : IntegrationEvent
{
}