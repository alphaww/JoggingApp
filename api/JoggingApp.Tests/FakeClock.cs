using JoggingApp.Core.Clock;

namespace JoggingApp.Tests
{
    public class FakeClock : IClock
    {
        private readonly DateTime _dateTime;
        public FakeClock(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
        public DateTime Now => _dateTime;
    }
}
