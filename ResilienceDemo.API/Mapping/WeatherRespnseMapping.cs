using ResilienceDemo.API.DTOs;
using ResilienceDemo.API.External;

namespace ResilienceDemo.API.Mapping
{
	public static class WeatherRespnseMapping
	{
		public static CurrentWeatherResponse? MapToCurrentWeatherResponse(this OpenWeatherResponse response, string units)
		{
			if (response is null)
			{
				return null;
			}

			return new CurrentWeatherResponse
			{
				WeatherCondition = new WeatherCondition
				{
					Temperature = response.Main.Temp,
					FeelsLike = response.Main.FeelsLike,
					Pressure = response.Main.Pressure,
					Humidity = response.Main.Humidity,
					SeaLevelPressure = response.Main.SeaLevelPressure,
					GroundLevelPressure = response.Main.GroundLevelPressure,
					Cloudiness = response.Clouds.Cloudiness,
					WindSpeed = response.Wind.Speed,
					WindDirection = response.Wind.Direction,
					WindGust = response.Wind.Gust,
					Visibility = response.Visibility,
					Condition = response.Weather.FirstOrDefault()?.Main ?? string.Empty
				},
				LocationData = new LocationData
				{
					City = response.Name,
					CountryCode = response.Sys.CountryCode,
					Latitude = response.Coord.Lat,
					Longitude = response.Coord.Lon,
					Sunrise = DateTimeOffset.FromUnixTimeSeconds(response.Sys.Sunrise).ToOffset(TimeSpan.FromSeconds(response.Timezone)),
					Sunset = DateTimeOffset.FromUnixTimeSeconds(response.Sys.Sunset).ToOffset(TimeSpan.FromSeconds(response.Timezone))
				},
				Units = units
			};
		}

		public static ForecastResponse? MapToForecastResponse(this OpenWeatherForecastResponse response, string units)
		{
			if (response is null)
			{
				return null;
			}
			var weatherConditions = response.List.Select(item => new ForecastedWeatherCondition
			{
				Temperature = item.Main.Temp,
				FeelsLike = item.Main.FeelsLike,
				Pressure = item.Main.Pressure,
				Humidity = item.Main.Humidity,
				SeaLevelPressure = item.Main.SeaLevelPressure,
				GroundLevelPressure = item.Main.GroundLevelPressure,
				Cloudiness = item.Clouds.Cloudiness,
				WindSpeed = item.Wind.Speed,
				WindDirection = item.Wind.Direction,
				WindGust = item.Wind.Gust,
				Visibility = item.Visibility,
				Condition = item.Weather.FirstOrDefault()?.Main ?? string.Empty,
				DateTime = DateTimeOffset.FromUnixTimeSeconds(item.Dt).ToOffset(TimeSpan.FromSeconds(response.City.Timezone))
			}).ToList();

			return new ForecastResponse
			{
				WeatherConditions = weatherConditions,
				LocationData = new LocationData
				{
					City = response.City.Name,
					CountryCode = response.City.CountryCode,
					Latitude = response.City.Coord.Lat,
					Longitude = response.City.Coord.Lon,
					Sunrise = DateTimeOffset.FromUnixTimeSeconds(response.City.Sunrise).ToOffset(TimeSpan.FromSeconds(response.City.Timezone)),
					Sunset = DateTimeOffset.FromUnixTimeSeconds(response.City.Sunset).ToOffset(TimeSpan.FromSeconds(response.City.Timezone))
				},
				Units = units
			};
		}
	}
}
