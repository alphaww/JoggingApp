using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace JoggingApp.Core.Outbox
{
    public class OutboxMessage
    {
        private OutboxMessage()
        {
        }

        public OutboxMessage(IEvent @event)
        {
            Id = Guid.NewGuid();

            EventType = @event switch
            {
                DomainEventBase => EventType.DomainEvent,
                IntegrationEventBase => EventType.IntegrationEvent
            };

            EventState = OutboxMessageState.Ready;
            Type = @event.GetType().ToString();
            OccurredOnUtc = DateTime.UtcNow;
            ProcessedOnUtc = null;
            Error = null;

            Content = JsonConvert.SerializeObject(
                @event,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }

        public Guid Id { get; }

        public string Type { get; }

        public string Content { get; }

        public EventType EventType { get; }

        public OutboxMessageState EventState { get; private set; }

        public DateTime OccurredOnUtc { get; }

        public DateTime? ProcessedOnUtc { get; private set; }

        public string Error { get; private set; }

        public void SetEventState(OutboxMessageState eventState, string error = null)
        {
            ProcessedOnUtc = DateTime.UtcNow;
            EventState = eventState;
            Error = error;
        }
    }
}
