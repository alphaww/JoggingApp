using System.Linq;
using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;
using JoggingApp.Core.Users;

namespace JoggingApp.Users.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IUserStorage _userStorage;
        private readonly IEventBus _eventBus;
        private readonly UserRegisteredEmailTemplateRenderer _userRegisteredEmailTemplateRenderer;
        public UserRegisteredEventHandler(IUserStorage userStorage, IEventBus eventBus, UserRegisteredEmailTemplateRenderer userRegisteredEmailTemplateRenderer)
        {
            _userStorage = userStorage;
            _eventBus = eventBus;
            _userRegisteredEmailTemplateRenderer = userRegisteredEmailTemplateRenderer;
        }

        public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
        {
            var user = await _userStorage.FindByEmailAsync(@event.Email, cancellationToken);
            var emailTemplate = await _userRegisteredEmailTemplateRenderer.RenderForUserActivationToken(user.ActivationTokens.Single());
            _eventBus.Publish(new SendEmailIntegrationEvent(@event.Email, "Confirm account", emailTemplate));
        }
    }
}
