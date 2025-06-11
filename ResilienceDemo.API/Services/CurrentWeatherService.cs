using Microsoft.AspNetCore.WebUtilities;
using Polly;
using Polly.Registry;
using ResilienceDemo.API.DTOs;
using ResilienceDemo.API.External;
using ResilienceDemo.API.Mapping;

namespace ResilienceDemo.API.Services
{
	public class CurrentWeatherService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly string _openWeatherApiKey;
		private static readonly ResiliencePipeline<HttpResponseMessage> _pipeline = CreateResiliencePipeline();

		private readonly ResiliencePipeline<HttpResponseMessage> _defaultPipeline = CreateResiliencePipeline();

		public CurrentWeatherService
			(IHttpClientFactory httpClientFactory,
			IConfiguration configuration,
			ResiliencePipelineProvider<string> pipelineProvider)
		{
			_httpClientFactory = httpClientFactory;
			_openWeatherApiKey = configuration["OpenWeather:ApiKey"] ?? throw new NullReferenceException(nameof(_openWeatherApiKey));
			_defaultPipeline = pipelineProvider.GetPipeline<HttpResponseMessage>("injected-pipeline") ?? throw new NullReferenceException(nameof(_defaultPipeline));
		}

		public async Task<CurrentWeatherResponse?> GetCurrentWeatherByCityAsync(WeatherByCityRequestDTO request)
		{
			if (string.IsNullOrEmpty(request.City))
			{
				return null;
			}
			var url = $"https://api.openweathermap.org/data/2.5/weather";
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

			QueryHelpers.AddQueryString(url, queryParams);

			var uri = new Uri(QueryHelpers.AddQueryString(url, queryParams));

			var client = _httpClientFactory.CreateClient();

			var response = await _pipeline.ExecuteAsync(async ct => await client.GetAsync(uri, ct));

			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var openWeatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
			return openWeatherResponse?.MapToCurrentWeatherResponse(request.Units ?? "c") ?? null;
		}

		public async Task<CurrentWeatherResponse?> GetCurrentWeatherByCoordinatesAsync(WeatherByCoordinatesRequestDTO request)
		{
			var url = $"https://api.openweathermap.org/data/2.5/weather?lat={request.Latitude}&lon={request.Longitude}";
			var queryParams = new Dictionary<string, string?>();

			var degreeUnits = request.Units switch
			{
				"f" => "imperial",
				"k" => "standard",
				_ => "metric"
			};

			queryParams.Add("units", degreeUnits);
			queryParams.Add("appid", _openWeatherApiKey);

			QueryHelpers.AddQueryString(url, queryParams);

			var uri = new Uri(QueryHelpers.AddQueryString(url, queryParams));

			var client = _httpClientFactory.CreateClient();

			var response = await _defaultPipeline.ExecuteAsync(async ct => await client.GetAsync(uri, ct));
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var openWeatherResponse = await response.Content.ReadFromJsonAsync<OpenWeatherResponse>();
			return openWeatherResponse?.MapToCurrentWeatherResponse(request.Units ?? "c") ?? null;
		}

		private static ResiliencePipeline<HttpResponseMessage> CreateResiliencePipeline()
		{
			var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
				.AddResilienceStrategy();// See in ResiliencePipelineBuilderExtensions class
			pipeline.AddChaosStrategy();// See in ResiliencePipelineBuilderExtensions class

			return pipeline.Build();
		}
	}
}
