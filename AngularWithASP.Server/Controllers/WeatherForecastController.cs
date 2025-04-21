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
		public List<Weather> Get([FromQuery] string lon, [FromQuery] string lat)
		{
			string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&daily=temperature_2m_max,temperature_2m_min&temperature_unit=fahrenheit";
			HttpClient client = new HttpClient();
			Uri baseUri = new Uri(url);
			client.BaseAddress = baseUri;
			client.DefaultRequestHeaders.Clear();
			client.DefaultRequestHeaders.ConnectionClose = true;

			try
			{
				var task = client.GetAsync(baseUri);
				var response = task.Result;
				response.EnsureSuccessStatusCode();
				string responseBody = response.Content.ReadAsStringAsync().Result;
				Console.WriteLine(responseBody);

				var jobject = JObject.Parse(responseBody);

				var weatherRaw = JsonSerializer.Deserialize<WeatherForecast>(jobject["daily"].ToString());
				var weather = new List<Weather>();

				for(int i = 0; i< weatherRaw.Time.Count() ; i++)
				{
					weather.Add(new Weather
					{
						Time = weatherRaw.Time[i],
						LowTemp = weatherRaw.LowTemp[i],
						HighTemp = weatherRaw.HighTemp[i]
					});
				}

				return weather ?? new List<Weather>();
			}
			catch (Exception e)
			{
				Console.Out.WriteLine("-----------------");
				Console.Out.WriteLine(e.Message);

				return new List<Weather>();
			}
		}
	}
}
