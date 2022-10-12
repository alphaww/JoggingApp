using System;
using JoggingApp.Core.Jogs;

namespace JoggingApp.Jogs
{
    public class JogDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Distance { get; set; }
        public RunningTimeDto Time { get; set; }

        public double AverageSpeed
        {
            get
            {
                var timeSpan = Time.ToTimeSpan();
                return (Distance / 1000.0) / timeSpan.TotalHours;
            }
        }

        public string FormattedTime
        {
            get
            {
                return $"{Time.Hours} hours, {Time.Minutes} minutes, {Time.Seconds} seconds";
            }
        }

        public JogLocationDto Location { get; set; }

        public JogDto(Jog jog)
        {
            Id = jog.Id;
            Date = jog.Date;
            Distance = jog.Distance;
            Time = new RunningTimeDto
            {
                Hours = jog.Time.Hours,
                Minutes = jog.Time.Minutes,
                Seconds = jog.Time.Seconds
            };

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
