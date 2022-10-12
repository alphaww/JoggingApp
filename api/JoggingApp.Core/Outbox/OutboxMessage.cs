using System.Runtime.InteropServices;

namespace JoggingApp.Core.Outbox
{
    public class OutboxMessage
    {
        private OutboxMessage()
        {

        }
        public OutboxMessage(string type, string content, DateTime occurredOn, DateTime? processedOnUtc = null, string error = null)
        {
            Id = Guid.NewGuid();
            Type = type;
            Content = content;
            OccurredOnUtc = occurredOn;
            ProcessedOnUtc = processedOnUtc;
            Error = error;
        }

        public Guid Id { get; }

        public string Type { get; }

        public string Content { get; }

        public DateTime OccurredOnUtc { get; }

        public DateTime? ProcessedOnUtc { get;  }

        public string Error { get; }
    }
}
