using Microsoft.AspNetCore.WebUtilities;
using ResilienceDemo.API.DTOs;
using ResilienceDemo.API.External;
using ResilienceDemo.API.Mapping;

namespace ResilienceDemo.API.Services
{
	public class V2WeatherService
	{
		private readonly string _openWeatherApiKey;
		private readonly HttpClient _httpClient;

		public V2WeatherService(IConfiguration configuration, HttpClient httpClient)
		{
			_openWeatherApiKey = configuration["OpenWeather:ApiKey"] ?? throw new NullReferenceException(nameof(_openWeatherApiKey));
			_httpClient = httpClient ?? throw new NullReferenceException(nameof(httpClient));
		}

		public async Task<CurrentWeatherResponse?> GetCurrentWeatherByCityAsync(WeatherByCityRequestDTO request)
		{
			if (string.IsNullOrEmpty(request.City))
			{
				return null;
			}

			var path = $"weather";

			var queryParams = new Dictionary<string, string?>();
			var qParam = request.City;

			if (!string.IsNullOrEmpty(request.StateCode))
			{
				qParam += $",{request.StateCode}";
			}

			if (!string.IsNullOrEmpty(request.CountryCode))
			{
				qParam += $",{request.CountryCode}";
			}

			queryParams.Add("q", qParam);
			var degreeUnits = request.Units switch
			{
				"f" => "imperial",
				"k" => "standard",
				_ => "metric"
			};

			queryParams.Add("units", degreeUnits);
			queryParams.Add("appid", _openWeatherApiKey);

			var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(path, queryParams));

			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var openWeatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
			return openWeatherResponse?.MapToCurrentWeatherResponse(request.Units ?? "c") ?? null;
		}

		public async Task<CurrentWeatherResponse?> GetCurrentWeatherByCoordinatesAsync(WeatherByCoordinatesRequestDTO request)
		{
			var path = $"weather?lat={request.Latitude}&lon={request.Longitude}";
			var queryParams = new Dictionary<string, string?>();

			var degreeUnits = request.Units switch
			{
				"f" => "imperial",
				"k" => "standard",
				_ => "metric"
			};

			queryParams.Add("units", degreeUnits);
			queryParams.Add("appid", _openWeatherApiKey);

			var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(path, queryParams));

			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var openWeatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
			return openWeatherResponse?.MapToCurrentWeatherResponse(request.Units ?? "c") ?? null;
		}

		public async Task<ForecastResponse?> GetForecastByCityAsync(WeatherByCityRequestDTO request)
		{
			if (string.IsNullOrEmpty(request.City))
			{
				return null;
			}
			var path = $"forecast";
			var queryParams = new Dictionary<string, string?>();

			var qParam = request.City;

			if (!string.IsNullOrEmpty(request.StateCode))
			{
				qParam += $",{request.StateCode}";
			}
			if (!string.IsNullOrEmpty(request.CountryCode))
			{
				qParam += $",{request.CountryCode}";
			}

			queryParams.Add("q", qParam);

			var degreeUnits = request.Units switch
			{
				"f" => "imperial",
				"k" => "standard",
				_ => "metric"
			};

			queryParams.Add("units", degreeUnits);
			queryParams.Add("appid", _openWeatherApiKey);

			var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(path, queryParams));
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var openWeatherForecastResponse = await response.Content.ReadFromJsonAsync<OpenWeatherForecastResponse>();
			return openWeatherForecastResponse?.MapToForecastResponse(request.Units ?? "c") ?? null;
		}

		public async Task<ForecastResponse?> GetForecastByCoordinatesAsync(WeatherByCoordinatesRequestDTO request)
		{
			var path = $"forecast?lat={request.Latitude}&lon={request.Longitude}";
			var queryParams = new Dictionary<string, string?>();

			var degreeUnits = request.Units switch
			{
				"f" => "imperial",
				"k" => "standard",
				_ => "metric"
			};

			queryParams.Add("units", degreeUnits);
			queryParams.Add("appid", _openWeatherApiKey);

			var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString(path, queryParams));
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var openWeatherForecastResponse = await response.Content.ReadFromJsonAsync<OpenWeatherForecastResponse>();
			return openWeatherForecastResponse?.MapToForecastResponse(request.Units ?? "c") ?? null;
		}
	}
}
