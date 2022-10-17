using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.Core.Jogs;
using JoggingApp.Core.Weather;
using System.Threading;
using System.Threading.Tasks;

namespace JoggingApp.Jogs.EventHandlers
{
    public class JogLocationSetEventHandler : IDomainEventHandler<JogLocationSetDomainEvent>
    {
        private  readonly IWeatherService _weatherService;
        private readonly IJogStorage _jogStorage;
        public JogLocationSetEventHandler(IWeatherService weatherService, IJogStorage jogStorage)
        {
            _weatherService = weatherService;
            _jogStorage = jogStorage;
        }

        public async Task Handle(JogLocationSetDomainEvent @event, CancellationToken cancellationToken)
        { 
            var weatherInfo = await _weatherService.FetchWeatherInfoAsync(@event.Coordinates, cancellationToken);
           var jogToUpdate = await _jogStorage.GetByJogIdAsync(@event.JogId, cancellationToken);
           jogToUpdate.SetLocationDetail(@event.Coordinates, weatherInfo);
           await _jogStorage.UpdateAsync(jogToUpdate, cancellationToken);
        }
    }
}
