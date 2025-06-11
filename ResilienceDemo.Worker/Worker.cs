namespace ResilienceDemo.Worker
{
	public class Worker : BackgroundService
	{
		private readonly WeatherClient _weatherClient;

		public Worker(WeatherClient weatherClient)
		{
			_weatherClient = weatherClient;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.WhenAll(
					_weatherClient.CallToCurrent(),
					_weatherClient.CallToForecast()
				);

				await Task.Delay(2000, stoppingToken);
			}
		}
	}
}
