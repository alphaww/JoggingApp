namespace JoggingApp.Core.Jogs
{
    public class JogLocation
    {
        public JogLocation(Jog jog, double latitude, double longitude, string location, string description, float temperature, float feelsLike)
        {
            Jog = jog;
            Latitude = latitude;
            Longitude = longitude;
            Location = location;
            Description = description;
            Temperature = temperature;
            FeelsLikeTemperature = feelsLike;          
        }

        public JogLocation()
        {
        }

        public Guid JogId { get; private set; }
        public Jog Jog { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Location { get; private set; }
        public string Description { get; private set; }
        public float Temperature { get; private set; }
        public float FeelsLikeTemperature { get; private set; }
    }
}
