using JoggingApp.Core;
using JoggingApp.Core.Jog.DomainEvents;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Jogs.EventHandlers
{
    internal sealed class JogLocationSetEventHandler : IDomainEventHandler<JogLocationSetDomainEvent>
    {
        public JogLocationSetEventHandler()
        {
        }

        public async Task Handle(JogLocationSetDomainEvent @event, CancellationToken cancellationToken)
        {

        }
    }
}
