using JoggingApp.BuildingBlocks.EventBus.Abstractions;
using JoggingApp.IntegrationEvents;

namespace JoggingApp.EemailService
{
    public class SendEmailIntegrationEventHandler : IIntegrationEventHandler<SendEmailIntegrationEvent>
    {
        private readonly IEmailSender _emailSender;

        public SendEmailIntegrationEventHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task Handle(SendEmailIntegrationEvent @event)
        {
            await _emailSender.SendAsync(new MailMessage(@event.Email, @event.Subject, @event.Body));
        }
    }
}
