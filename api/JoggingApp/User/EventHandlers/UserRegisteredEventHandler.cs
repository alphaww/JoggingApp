using System.Linq;
using JoggingApp.Core.Email;
using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.Users;
using System.Threading;
using System.Threading.Tasks;
using JoggingApp.Core.Users;

namespace JoggingApp.Jogs.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IUserStorage _userStorage;
        private readonly IEmailSender _emailSender;
        private readonly UserRegisteredEmailTemplateRenderer _userRegisteredEmailTemplateRenderer;
        public UserRegisteredEventHandler(IUserStorage userStorage, IEmailSender emailSender, UserRegisteredEmailTemplateRenderer userRegisteredEmailTemplateRenderer)
        {
            _userStorage = userStorage;
            _emailSender = emailSender;
            _userRegisteredEmailTemplateRenderer = userRegisteredEmailTemplateRenderer;
        }

        public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
        {
            var user = await _userStorage.FindByEmailAsync(@event.Email, cancellationToken);
            var emailTemplate = await _userRegisteredEmailTemplateRenderer.RenderForUserActivationToken(user.ActivationTokens.Single());
            await _emailSender.SendAsync(new MailMessage(user.Email, "Confirm account", emailTemplate));
        }
    }
}
