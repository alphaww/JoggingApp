namespace JoggingApp.Core.Email
{
    public interface IEmailSender
    {
        Task SendAsync(MailMessage message);
    }
}
