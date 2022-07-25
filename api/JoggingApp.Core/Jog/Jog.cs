using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jogs
{
    public class Jog
    {
        public Jog(Guid userId, int distance, TimeSpan time)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Date = DateTime.UtcNow;
            Distance = distance;
            Time = time;
        }

        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
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

        public void AddLocationDetail(Coordinates coordinates, WeatherInfo weatherInfo)
        {
            JogLocation = new JogLocation(this, coordinates.Latitude,
                coordinates.Longitude, weatherInfo.Location, weatherInfo.Description,
                weatherInfo.Temperature, weatherInfo.FeelsLikeTemperature);
        }
    }
}
