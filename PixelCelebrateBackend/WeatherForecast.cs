namespace PixelCelebrateBackend
{
    //Clasa:
    public class WeatherForecast
    {
        //Getter and Setter;
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        //Lambda direct in definitia campului;
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        //? Null?
        public string? Summary { get; set; }
    }
}