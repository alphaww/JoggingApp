namespace JoggingApp.Jogs
{
    public class JogLocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public float Temperature { get; set; }
        public float FeelsLikeTemperature { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
    }
}
