# ResilienceDemo

This is a simple and modular .NET Web API project built as a resilience demo on top of a basic Weather API. It is designed to help study and test common resilience patterns such as:

 - **Retry**
 - **Timeout**
 - **Circuit Braker**

## Features

- Exposes endpoints for:
  - **Current Weather**
  - **Weather Forecast**
- Easily extendable to demonstrate resilience strategies

## Prerequisites

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download/dotnet/9.0)
- A free API key from [OpenWeatherMap](https://openweathermap.org/)

## Setup

1. Clone the repository:
   ```sh
   git clone https://github.com/steverzag/resilience-demo-template.git
   cd weather-api
   ```
2. Or add it to your [appsettings.json](ResilienceDemo.API/appsettings.json):
    ```json
    {
        //...
        "OpenWeather": {
        "ApiKey": "<YOUR_OPEN_WEATHER_MAP_API_KEY>"
        }   
    }
    ```

3. Restore and build the project:

    ```sh
    dotnet restore
    dotnet build
    ```

## Running the Application
To run the API locally:

    ```sh
    dotnet run --project ./ResilienceDemo.API/ResilienceDemo.API.csproj
    ```

Or to run the application using [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)

   ```sh
   dotnet run --project ./ResilienceDemo.AppHost/ResilienceDemo.AppHost.csproj
   ```

By default, the application will be available at `http://localhost:5000` (or `https://localhost:5001` for HTTPS).

## API Usage
Example endpoints:

 - GET /current/by-city?cityName=London — Get the current weather for a city

 - GET /forecast/by-city?cityName=London — Get the weather forecast for a city

 > Refer to the included [ResilienceDemo.API.http](ResilienceDemo.API/ResilienceDemo.API.http) file for more details.

When running with Aspire, the ResilienceDemo.Worker project is started alongside the API. It performs requests to the API every 2 seconds to simulate load and help observe the resilience strategies in action.

The parameters used by the Worker when calling the API can be tunned in the [./ResilienceDemo.Worker/WeatherClient class](ResilienceDemo.Worker/WeatherClient.cs), via its constant values:

![Worker weather client's constants](https://github.com/steverzag/docs-assets/blob/main/images/resilience-demo-worker-weather-client-consts.png)

The API exposes two versions of the weather endpoints:

 - v1 – Used to demonstrate how different custom resilience strategies can be implemented.
 - v2 – Uses the Standard Resilience Handler provided by .NET Resilience Strategies.

## References
 - [OpenWeatherMap API Docs](https://openweathermap.org/api)
 - [Polly Docs](https://www.pollydocs.org/index.html)
 - [.NET Resilience Strategies](https://learn.microsoft.com/en-us/dotnet/core/resilience/?tabs=dotnet-cli)





