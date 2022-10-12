using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jog.DomainEvents
{
    public sealed record JogLocationSetDomainEvent(Guid JogId, Coordinates Coordinates) : IDomainEvent
    {
    }
}
