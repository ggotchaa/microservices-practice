namespace WeatherAPI.Models
{
    public class WeatherResponse
    {
        public WeatherMain Main { get; set; }
        public WeatherDescription[] Weather { get; set; }
    }

    public class WeatherMain
    {
        public double Temp { get; set; }
    }

    public class WeatherDescription
    {
        public string Description { get; set; }
    }
}
