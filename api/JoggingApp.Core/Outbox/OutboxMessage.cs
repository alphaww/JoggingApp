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
            EventState = OutboxMessageState.ReadyForProcessing;
            OccurredOnUtc = occurredOn;
            ProcessedOnUtc = processedOnUtc;
            Error = error;
        }

        public Guid Id { get; }

        public string Type { get; }

        public string Content { get; }

        public OutboxMessageState EventState { get; }

        public DateTime OccurredOnUtc { get; }

        public DateTime? ProcessedOnUtc { get; private set; }

        public string Error { get; private set; }

        public void SetProcessed(string error)
        {
            ProcessedOnUtc = DateTime.UtcNow;
            Error = error;

        }
    }
}
