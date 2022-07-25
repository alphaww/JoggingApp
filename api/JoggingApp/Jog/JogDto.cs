using JoggingApp.Core.Jogs;
using JoggingApp.Core.Weather;
using JoggingApp.Infra;
using System;

namespace JoggingApp.Jogs
{
    public class JogDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public int Distance { get; set; }

        public string Time { get; set; }

        public double AverageSpeed
        {
            get
            {
                var timeSpan = Time.ToTimeSpan();
                return timeSpan.TotalHours / Distance;
            }
        }

        public JogLocationDto Location { get; set; }

        public JogDto(Jog jog)
        {
            Id = jog.Id;
            Date = jog.Date;
            Time = jog.Time.ToString();
            Distance = jog.Distance;

            if (jog.JogLocation is not null)
            {
                Location = new JogLocationDto
                {
                    Latitude = jog.JogLocation.Latitude,
                    Longitude = jog.JogLocation.Longitude,
                    Description = jog.JogLocation.Description,
                    FeelsLikeTemperature = jog.JogLocation.FeelsLikeTemperature,
                    Location = jog.JogLocation.Location,
                    Temperature = jog.JogLocation.Temperature
                };
            }
        }
    }
}
