using ResilienceDemo.Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<WeatherClient>(client => 
{
    client.BaseAddress = new Uri("http://localhost:5000/");
});
builder.Services.AddHostedService<Worker>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
