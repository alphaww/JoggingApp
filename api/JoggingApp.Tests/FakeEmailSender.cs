
using JoggingApp.Core.Email;

namespace JoggingApp.Tests
{
    public class FakeEmailSender : IEmailSender
    {
        public bool WasCalled { get; private set; }
        public Task SendAsync(MailMessage message)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }
}
