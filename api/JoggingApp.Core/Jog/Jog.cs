using JoggingApp.Core.Clock;
using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jogs
{
    public class Jog : Entity
    {
        public static Jog Create(Guid userId, int distance, TimeSpan time, Coordinates coordinates, IClock clock)
        {
            return new Jog(userId, distance, time, clock.Now.Date, coordinates);
        }

        private Jog(Guid userId, int distance, TimeSpan time, DateTime date, Coordinates coordinates) : base(Guid.NewGuid())
        {
            UserId = userId;
            Date = date;
            Distance = distance;
            Time = time;

            if (coordinates is not null)
                RaiseDomainEvent(new JogLocationSetDomainEvent(Id, coordinates));
        }

        private Jog()
        {
        }

        public DateTime Date { get; }
        public int Distance { get; private set; }
        public TimeSpan Time { get; private set; }
        public User User { get; private set; }
        public Guid UserId { get; private set; }

        public JogLocation JogLocation { get; private set; }

        public void Update(int distance, TimeSpan time)
        {
            Distance = distance;
            Time = time;
        }

        public void SetLocationDetail(Coordinates coordinates, WeatherInfo weatherInfo)
        {
            JogLocation = new JogLocation(this, coordinates.Latitude,coordinates.Longitude, weatherInfo.Location, weatherInfo.Description,
                weatherInfo.Temperature, weatherInfo.FeelsLikeTemperature);
        }
    }
}
