using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.Core.Outbox;
using JoggingApp.Core.Users;
using JoggingApp.IntegrationEvents;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Users.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IUserStorage _userStorage;
        private readonly IOutboxStorage _outboxStorage;
        private readonly UserRegisteredEmailTemplateRenderer _userRegisteredEmailTemplateRenderer;
        public UserRegisteredEventHandler(IUserStorage userStorage, IOutboxStorage outboxStorage, UserRegisteredEmailTemplateRenderer userRegisteredEmailTemplateRenderer)
        {
            _userStorage = userStorage;
            _outboxStorage = outboxStorage;
            _userRegisteredEmailTemplateRenderer = userRegisteredEmailTemplateRenderer;
        }

        public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
        {
            var emailTemplate = await _userRegisteredEmailTemplateRenderer.RenderForUserActivationToken(@event.User.ActivationTokens.Single());
            var integrationEvent = new SendEmailIntegrationEvent(@event.User.Email, "Confirm account", emailTemplate);
            await _outboxStorage.InsertOutboxEventAsync(new OutboxMessage(integrationEvent));
        }
    }
}
