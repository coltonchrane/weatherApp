using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace weatherApp.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private readonly ILogger<WeatherForecastController> _logger;
		private readonly IConfiguration _configuration;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public WeatherPage Get([FromQuery] string lon, [FromQuery] string lat)
		{
			var apiKey = _configuration["Geocoding:ApiKey"];
			var service = new WeatherService(lon, lat, apiKey);

			return new WeatherPage
			{
				Location = service.GetLocation(),
				Forecasts = service.GetForecasts()
			};
		}
	}
}
