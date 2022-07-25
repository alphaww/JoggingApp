namespace JoggingApp.Core.Email
{
    public class MailMessage
    {
        public MailMessage(string mailTo, string subject, string body, ICollection<Attachment> att = null)
        {
            MailTo = new List<string>() { mailTo };
            Attachments = att ?? new List<Attachment>();
            MailFrom = null;
            Cc = null;
            Subject = subject;
            Body = body;
        }

        public MailMessage(string[] mailTo, string subject, string body, ICollection<Attachment> att = null, ICollection<string> replyToAddresses = null, bool useBcc = false)
        {
            MailTo = new List<string>();
            Attachments = att ?? new List<Attachment>();
            MailFrom = null;
            Cc = null;
            UseBcc = useBcc;
            Subject = subject;
            Body = body;
            ReplyToAddresses = replyToAddresses ?? new List<string>();

            foreach (var addr in mailTo)
                MailTo.Add(addr);

        }

        public ICollection<string> MailTo { get; private set; }
        public string MailFrom { get; private set; }
        public string Cc { get; private set; }
        public bool UseBcc { get; set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public ICollection<Attachment> Attachments { get; private set; }
        public ICollection<string> ReplyToAddresses { get; private set; }

        public void AddAttachment(string fileName, byte[] data)
        {
            Attachments.Add(new Attachment(fileName, data));
        }
    }
}