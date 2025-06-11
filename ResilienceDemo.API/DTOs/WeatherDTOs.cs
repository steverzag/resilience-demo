namespace ResilienceDemo.API.DTOs
{
	public record CurrentWeatherDTO
	{

	}

	public record WeatherByCityRequestDTO : CurrentWeatherRequestDTO
	{
		public string City { get; set; } = string.Empty;
		public string? StateCode { get; set; }
		public string? CountryCode { get; set; }
	}

	public record WeatherByCoordinatesRequestDTO : CurrentWeatherRequestDTO
	{
		public double Longitude { get; set; }
		public double Latitude { get; set; }
	}

	public record CurrentWeatherRequestDTO
	{
		public string? Units { get; set; }
	}

	public record CurrentWeatherResponse : WeatherResponse
	{
		public WeatherCondition WeatherCondition { get; set; }

	}

	public record ForecastResponse : WeatherResponse
	{
		public IEnumerable<ForecastedWeatherCondition> WeatherConditions { get; set; }
	}

	public record WeatherResponse
	{
		public LocationData LocationData { get; set; }
		public string Units { get; set; }
	}

	public record WeatherCondition
	{
		public double Temperature { get; set; }
		public double FeelsLike { get; set; }
		public int Pressure { get; set; }
		public int Humidity { get; set; }
		public int SeaLevelPressure { get; set; }
		public int GroundLevelPressure { get; set; }
		public double Cloudiness { get; set; }
		public double WindSpeed { get; set; }
		public int WindDirection { get; set; }
		public double WindGust { get; set; }
		public int Visibility { get; set; }
		public string Condition { get; set; } = string.Empty;
	}

	public record ForecastedWeatherCondition : WeatherCondition
	{
		public DateTimeOffset DateTime { get; set; }
	}

	public record LocationData
	{
		public string City { get; set; } = string.Empty;
		public string? CountryCode { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTimeOffset Sunrise { get; set; }
		public DateTimeOffset Sunset { get; set; }
	}
}
