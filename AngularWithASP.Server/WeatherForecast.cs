using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace weatherApp.Server
{
	public class WeatherForecast
	{
		[JsonPropertyName("time")]
		public List<string> Time { get; set; }

		[JsonPropertyName("temperature_2m_max")]
		public List<double> HighTemp { get; set; }

		[JsonPropertyName("temperature_2m_min")]
		public List<double> LowTemp { get; set; }
	}

	public class Weather
	{
		public string Time { get; set; }
		public double HighTemp { get; set; }

		public double LowTemp { get; set; }	
	}

	public class WeatherPage
	{
		public string Location { get; set; }
		public List<Weather> Forecasts { get; set; }
	}

	public class WeatherService
	{
		private string Longitude;
		private string Latitude;
		private string ApiKey;
		public WeatherService(string lon, string lat, string apiKey)
		{
			Longitude = lon;
			Latitude = lat;
			ApiKey = apiKey;
		}

		public List<Weather> GetForecasts()
		{
			string url = $"https://api.open-meteo.com/v1/forecast?" +
			$"latitude={Latitude}&" +
			$"longitude={Longitude}&" +
			$"daily=temperature_2m_max,temperature_2m_min&" +
			$"temperature_unit=fahrenheit";

			var response = MakeApiRequest(url);
			response.EnsureSuccessStatusCode();
			string responseBody = response.Content.ReadAsStringAsync().Result;
			Console.WriteLine(responseBody);

			var jobject = JObject.Parse(responseBody);

			var weatherRaw = JsonSerializer.Deserialize<WeatherForecast>(jobject["daily"].ToString());
			var weather = new List<Weather>();

			for (int i = 0; i < weatherRaw.Time.Count(); i++)
			{
				weather.Add(new Weather
				{
					Time = weatherRaw.Time[i],
					LowTemp = weatherRaw.LowTemp[i],
					HighTemp = weatherRaw.HighTemp[i]
				});
			}

			return weather;
		}
		public string GetLocation()
		{
			string url = $"https://geocode.maps.co/reverse?" +
				$"lat={Latitude}&" +
				$"lon={Longitude}&" +
				$"api_key={ApiKey}";

			var response = MakeApiRequest(url);
			response.EnsureSuccessStatusCode();
			string responseBody = response.Content.ReadAsStringAsync().Result;

			Console.WriteLine(responseBody);

			var jobject = JObject.Parse(responseBody);

			return jobject["display_name"].ToString();

		}
		private HttpResponseMessage MakeApiRequest(string url)
		{
			using (HttpClient client = new HttpClient())
			{
				Uri baseUri = new Uri(url);
				client.BaseAddress = baseUri;
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.ConnectionClose = true;
				try
				{
					var task = client.GetAsync(baseUri);
					return task.Result;
				}
				catch(Exception e)
				{
					Console.Out.WriteLine("-----------------");
					Console.Out.WriteLine(e.Message);

					return new HttpResponseMessage();
				}
			}
		}
	}
}
