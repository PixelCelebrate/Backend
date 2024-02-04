using Microsoft.AspNetCore.Mvc;

namespace PixelCelebrateBackend.Controllers
{
    //Ruta:
    [ApiController]
    [Route("[controller]")]
    //Implement or Extend? Controller Base:
    public class WeatherForecastController : ControllerBase
    {
        //Liste[]; Creeare lista string:
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //Log:
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //HTTP request get: Nu stiu unde este referentiat:
        [HttpGet(Name = "GetWeatherForecast")]
        //Asincronitate?
        public IEnumerable<WeatherForecast> Get()
        {
            //Conversii:
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}