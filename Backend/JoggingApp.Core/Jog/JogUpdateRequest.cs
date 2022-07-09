namespace JoggingApp.Core
{
    public class JogUpdateRequest
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public double Distance { get; set; }

        public int Time { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
