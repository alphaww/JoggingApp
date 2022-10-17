namespace JoggingApp.EemailService
{
    public interface IEmailSender
    {
        Task SendAsync(MailMessage message);
    }
}
