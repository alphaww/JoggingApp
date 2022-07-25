
using JoggingApp.Core.Email;

namespace JoggingApp.Tests
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendAsync(MailMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
