using JoggingApp.BuildingBlocks.EventBus.Events;
using JoggingApp.Core;

namespace JoggingApp.IntegrationEvents;

public record SendEmailIntegrationEvent(string Email, string Subject, string Body) : IntegrationEventBase
{
}