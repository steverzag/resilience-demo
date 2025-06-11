using System.Text.Json.Serialization;

namespace ResilienceDemo.API.External
{
	public class OpenWeatherResponse
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Visibility { get; set; }
		public int Timezone { get; set; }
		public int Dt { get; set; }
		public IEnumerable<WeatherInfo> Weather { get; set; }
		public MainWeatherInfo Main { get; set; }
		public OpenWeatherCoordinates Coord { get; set; }
		public OpenWeatherClouds Clouds { get; set; }
		public OpenWeatherWind Wind { get; set; }
		public OpenWeatherSys Sys { get; set; }

	}

	public class WeatherInfo
	{
		public int Id { get; set; }
		public string Main { get; set; }
		public string Description { get; set; }
		public string Icon { get; set; }
	}

	public class MainWeatherInfo
	{
		public double Temp { get; set; }
		[JsonPropertyName("feels_like")]
		public double FeelsLike { get; set; }
		[JsonPropertyName("temp_min")]
		public double TempMin { get; set; }
		[JsonPropertyName("temp_max")]
		public double TempMax { get; set; }
		public int Pressure { get; set; }
		public int Humidity { get; set; }
		[JsonPropertyName("sea_level")]
		public int SeaLevelPressure { get; set; }
		[JsonPropertyName("grnd_level")]
		public int GroundLevelPressure { get; set; }
	}

	public class OpenWeatherCoordinates
	{
		public double Lon { get; set; }
		public double Lat { get; set; }
	}

	public class OpenWeatherWind
	{
		[JsonPropertyName("speed")]
		public double Speed { get; set; }
		[JsonPropertyName("deg")]
		public int Direction { get; set; }
		[JsonPropertyName("gust")]
		public double Gust { get; set; }

	}

	public class OpenWeatherClouds
	{
		[JsonPropertyName("all")]
		public int Cloudiness { get; set; }
	}

	public class OpenWeatherSys
	{
		[JsonPropertyName("country")]
		public string CountryCode { get; set; } = string.Empty;
		public int Sunrise { get; set; }
		public int Sunset { get; set; }
	}

	public class OpenWeatherCity
	{
		public string Name { get; set; } = string.Empty;
		[JsonPropertyName("country")]
		public string CountryCode { get; set; } = string.Empty;
		public int Timezone { get; set; }
		public int Sunrise { get; set; }
		public int Sunset { get; set; }
		public OpenWeatherCoordinates Coord { get; set; }
	}

	public class OpenWeatherForecastResponse
	{
		public IEnumerable<OpenWeatherResponse> List { get; set; }
		public OpenWeatherCity City { get; set; }
	}
}
