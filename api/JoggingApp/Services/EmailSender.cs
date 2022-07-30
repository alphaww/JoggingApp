using JoggingApp.Core.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace JoggingApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromAddress;
        private readonly string _fromAddressTitle;
        private readonly string _replyFromAddressTitle;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _enableSsl;

        public EmailSender(IConfiguration configuration)
        {
            _smtpServer = configuration["Email:SmtpServer"];
            _ = int.TryParse(configuration["Email:SmtpPort"], out int port);
            _smtpPort = _smtpPort == 0 ? 25 : port;
            _fromAddress = configuration["Email:FromAddress"];
            _fromAddressTitle = configuration["Email:FromAddressTitle"];
            _replyFromAddressTitle = configuration["Email:ReplyFromAddressTitle"];
            _username = configuration["Email:SmtpUsername"];
            _password = configuration["Email:SmtpPassword"];
            _ = bool.TryParse(configuration["Email:EnableSsl"], out bool useSsl);
            _enableSsl = useSsl;
        }

        private MimeMessage CreateMimeMessage(MailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_fromAddressTitle, _fromAddress));

            if (message.UseBcc)
                foreach (var addr in message.MailTo)
                    mimeMessage.Bcc.Add(MailboxAddress.Parse(addr));
            else
                foreach (var addr in message.MailTo)
                    mimeMessage.To.Add(MailboxAddress.Parse(addr));

            mimeMessage.Subject = message.Subject;

            foreach (var replyTo in message.ReplyToAddresses)
                mimeMessage.ReplyTo.Add(new MailboxAddress(_replyFromAddressTitle, replyTo));


            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Body
            };

            foreach (var att in message.Attachments)
                bodyBuilder.Attachments.Add(att.FileName, att.Data);

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            return mimeMessage;
        }

        public async Task SendAsync(MailMessage message)
        {
            var mimeMessage = CreateMimeMessage(message);

            using var client = new SmtpClient();
            if (!_enableSsl)
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                // Allow SSLv3.0 and all versions of TLS
                client.SslProtocols =
                    SslProtocols.Ssl3 |
                    SslProtocols.Tls |
                    SslProtocols.Tls11 |
                    SslProtocols.Tls12 |
                    SslProtocols.Tls13;
            }

            await client.ConnectAsync(_smtpServer, _smtpPort, _enableSsl);
            if (!string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password))
            {
                client.Authenticate(_username, _password);
            }
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
