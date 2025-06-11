using Polly;
using ResilienceDemo.API;
using ResilienceDemo.API.Endpoints.Configuration;
using ResilienceDemo.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Playing wth Different approaches to resilience strategies
builder.Services.AddHttpClient();

builder.Services.AddScoped<CurrentWeatherService>();
builder.Services.AddResiliencePipeline<string, HttpResponseMessage>("injected-pipeline", builder =>
{
	builder.AddResilienceStrategy();// See in ResiliencePipelineBuilderExtensions class
	builder.AddChaosStrategy();// See in ResiliencePipelineBuilderExtensions class
});

builder.Services.AddHttpClient<ForecastService>(client =>
{
	client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
})
	.AddResilienceHandler("integrated-pipeline", builder =>
	{
		builder.AddResilienceStrategy();// See in ResiliencePipelineBuilderExtensions class
		builder.AddChaosStrategy();// See in ResiliencePipelineBuilderExtensions class
	});

// Use as prefered resilience strategy for most of scenarios
builder.Services.AddHttpClient<V2WeatherService>(client =>
{
	client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
})
	.AddStandardResilienceHandler();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.RegisterEndpoints();

app.Run();