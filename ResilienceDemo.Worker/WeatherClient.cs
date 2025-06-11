
namespace ResilienceDemo.Worker
{
	public class WeatherClient
	{
		private readonly HttpClient _httpClient;

		//Edit these constants to change the location and api version for the weather API calls.
		private const double Lon = -80.1937; 
		private const double Lat = 25.7743;
		private const string ApiVersion = "v1";

		public WeatherClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task CallToCurrent()
		{
			var response = await _httpClient.GetAsync($"{ApiVersion}/current?lat={Lat}&lon={Lon}");
			Console.WriteLine($"Response Status {response.StatusCode}");
		}

		public async Task CallToForecast()
		{
			var response = await _httpClient.GetAsync($"{ApiVersion}/forecast?lat={Lat}&lon={Lon}");
			Console.WriteLine($"Response Status {response.StatusCode}");
		}
	}
}
