using System;

namespace JoggingApp.Jogs
{
    public class RunningTimeDto
    {
        public int Hours { get; set; }

        public int Minutes { get; set; }

        public int Seconds { get; set; }

        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Hours, Minutes, Seconds);
        }
    }
}
