using JoggingApp.BuildingBlocks.EventBus.Events;

namespace JoggingApp.Users.IntegrationEvents;

public record UserRegisteredIntegrationEvent(string Email) : IntegrationEvent
{
}