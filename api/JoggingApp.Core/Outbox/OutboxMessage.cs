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

        public OutboxMessageState EventState { get; private set; }

        public DateTime OccurredOnUtc { get; }

        public DateTime? ProcessedOnUtc { get; private set; }

        public string Error { get; private set; }

        public void SetProcessedSuccessfully()
        {
            ProcessedOnUtc = DateTime.UtcNow;
            EventState = OutboxMessageState.ProcessedSuccessfully;
        }

        public void SetFailed(string error)
        {
            ProcessedOnUtc = DateTime.UtcNow;
            EventState = OutboxMessageState.Failed;
            Error = error;
        }
    }
}
