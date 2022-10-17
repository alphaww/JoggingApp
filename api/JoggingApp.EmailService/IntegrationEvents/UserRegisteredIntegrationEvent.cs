using JoggingApp.BuildingBlocks.EventBus.Events;
using JoggingApp.EemailService;

namespace JoggingApp.Users.IntegrationEvents;

public record UserRegisteredIntegrationEvent(string Email) : IntegrationEvent
{
}