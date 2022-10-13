using System.Linq;
using JoggingApp.Core.Email;
using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.Users;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Jogs.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly UserRegisteredEmailTemplateRenderer _userRegisteredEmailTemplateRenderer;
        public UserRegisteredEventHandler(IEmailSender emailSender, UserRegisteredEmailTemplateRenderer userRegisteredEmailTemplateRenderer)
        {
            _emailSender = emailSender;
            _userRegisteredEmailTemplateRenderer = userRegisteredEmailTemplateRenderer;
        }

        public async Task Handle(UserRegisteredDomainEvent @event, CancellationToken cancellationToken)
        {
            var emailTemplate = await _userRegisteredEmailTemplateRenderer.RenderForUserActivationToken(@event.User.ActivationTokens.Single());
            await _emailSender.SendAsync(new MailMessage(@event.User.Email, "Confirm account", emailTemplate));
        }
    }
}
