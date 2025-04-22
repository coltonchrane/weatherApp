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

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public WeatherPage Get([FromQuery] string lon, [FromQuery] string lat)
		{
			var service = new WeatherService(lon, lat);

			return new WeatherPage
			{
				Location = service.GetLocation(),
				Forecasts = service.GetForecasts()
			};
		}
	}
}
