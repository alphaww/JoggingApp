using JoggingApp.Core.Clock;
using System;

namespace JoggingApp.Services
{
    public class SystemClock : IClock
    {
        public DateTime Now => DateTime.UtcNow;
    }
}
