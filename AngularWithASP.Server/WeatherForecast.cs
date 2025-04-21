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
}
