using System;

namespace JoggingApp.Infra
{
    public static class StringExtensions
    {
        public static TimeSpan ToTimeSpan(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s));
            }
            var sp = s.Split(":");
            return new TimeSpan(int.Parse(sp[0]), int.Parse(sp[1]), int.Parse(sp[2]));
        }
    }
}
