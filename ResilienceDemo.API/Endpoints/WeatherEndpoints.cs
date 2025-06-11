using ResilienceDemo.API.DTOs;
using ResilienceDemo.API.Endpoints.Configuration;
using ResilienceDemo.API.Services;

namespace ResilienceDemo.API.Endpoints
{
	public class WeatherEndpoints : IEndpoints
	{
		public void RegisterEndpoints(IEndpointRouteBuilder builder)
		{
			builder.MapGet("/v1/current", GetCurrentWeatherByCoordinates);
			builder.MapGet("/v1/current/by-city", GetCurrentWeatherByCity);
			builder.MapGet("/v1/forecast", GetForecastByCoordinates);
			builder.MapGet("/v1/forecast/by-city", GetForecastByCity);

			builder.MapGet("/v2/current", GetCurrentWeatherByCoordinatesV2);
			builder.MapGet("/v2/current/by-city", GetCurrentWeatherByCityV2);
			builder.MapGet("/v2/forecast", GetForecastByCoordinatesV2);
			builder.MapGet("/v2/forecast/by-city", GetForecastByCityV2);
		}

		public static async Task<IResult> GetCurrentWeatherByCity(CurrentWeatherService weatherService, string cityName, string? stateCode = null, string? countryCode = null, string? units = null)
		{
			var request = new WeatherByCityRequestDTO
			{
				City = cityName,
				StateCode = stateCode,
				CountryCode = countryCode,
				Units = units
			};

			var response = await weatherService.GetCurrentWeatherByCityAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetCurrentWeatherByCoordinates(CurrentWeatherService weatherService, double lon, double lat, string? units = null)
		{
			var request = new WeatherByCoordinatesRequestDTO
			{
				Longitude = lon,
				Latitude = lat,
				Units = units
			};

			var response = await weatherService.GetCurrentWeatherByCoordinatesAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetForecastByCity(ForecastService forecastService, string cityName, string? stateCode = null, string? countryCode = null, string? units = null)
		{
			var request = new WeatherByCityRequestDTO
			{
				City = cityName,
				StateCode = stateCode,
				CountryCode = countryCode,
				Units = units
			};

			var response = await forecastService.GetForecastByCityAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetForecastByCoordinates(ForecastService forecastService, double lon, double lat, string? units = null)
		{
			var request = new WeatherByCoordinatesRequestDTO
			{
				Longitude = lon,
				Latitude = lat,
				Units = units
			};

			var response = await forecastService.GetForecastByCoordinatesAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetCurrentWeatherByCityV2(V2WeatherService weatherService, string cityName, string? stateCode = null, string? countryCode = null, string? units = null)
		{
			var request = new WeatherByCityRequestDTO
			{
				City = cityName,
				StateCode = stateCode,
				CountryCode = countryCode,
				Units = units
			};

			var response = await weatherService.GetCurrentWeatherByCityAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetCurrentWeatherByCoordinatesV2(V2WeatherService weatherService, double lon, double lat, string? units = null)
		{
			var request = new WeatherByCoordinatesRequestDTO
			{
				Longitude = lon,
				Latitude = lat,
				Units = units
			};

			var response = await weatherService.GetCurrentWeatherByCoordinatesAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetForecastByCityV2(V2WeatherService forecastService, string cityName, string? stateCode = null, string? countryCode = null, string? units = null)
		{
			var request = new WeatherByCityRequestDTO
			{
				City = cityName,
				StateCode = stateCode,
				CountryCode = countryCode,
				Units = units
			};

			var response = await forecastService.GetForecastByCityAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}

		public static async Task<IResult> GetForecastByCoordinatesV2(V2WeatherService forecastService, double lon, double lat, string? units = null)
		{
			var request = new WeatherByCoordinatesRequestDTO
			{
				Longitude = lon,
				Latitude = lat,
				Units = units
			};

			var response = await forecastService.GetForecastByCoordinatesAsync(request);
			if (response == null)
			{
				return Results.NotFound();
			}

			return Results.Ok(response);
		}
	}
}
