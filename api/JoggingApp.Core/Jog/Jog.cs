﻿using System.Net.Http.Headers;
using JoggingApp.Core.Clock;
using JoggingApp.Core.Jog.DomainEvents;
using JoggingApp.Core.Users;
using JoggingApp.Core.Weather;

namespace JoggingApp.Core.Jogs
{
    public class Jog : Entity
    {
        public static Jog Create(Guid userId, int distance, TimeSpan time, IClock clock)
        {
            return new Jog(userId, distance, time, clock.Now.Date);
        }

        private Jog(Guid userId, int distance, TimeSpan time, DateTime date) : base(Guid.NewGuid())
        {
            UserId = userId;
            Date = date;
            Distance = distance;
            Time = time;
        }

        private Jog()
        {
        }

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

        public void SetLocationDetail(Coordinates coordinates)
        {
            if (coordinates is null)
            {
                return;
            }
            //JogLocation = new JogLocation(this, coordinates.Latitude,
            //    coordinates.Longitude, weatherInfo.Location, weatherInfo.Description,
            //    weatherInfo.Temperature, weatherInfo.FeelsLikeTemperature);

            RaiseDomainEvent(new JogLocationSetDomainEvent(Id, coordinates));
        }
    }
}
