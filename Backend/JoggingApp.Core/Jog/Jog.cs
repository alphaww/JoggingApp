namespace JoggingApp.Core
{
    public class Jog
    {
        public static Jog Create(Guid userId, DateTime date, double distance, double latitude, double longitude)
        {
            return new Jog(userId, date, distance, latitude, longitude);
        }

        public Jog(Guid userId, DateTime date, double distance, double latitude, double longitude)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Date = date;
            Distance = distance;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }

        public double Distance { get; private set; }

        public int Time { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public User User { get; private set; }

        public Guid UserId { get; private set; }

        public void Update(DateTime date, double distance, int time, double lattitude, double longitude)
        {
            Date = date;
            Distance = distance;
            Time = time;
            Latitude = lattitude;
            Longitude = longitude;
        }       
    }
}
