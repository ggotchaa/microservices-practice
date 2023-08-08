using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherAPI.Models;


[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IMongoCollection<WeatherData> _weatherCollection;
    private readonly IHttpClientFactory _httpClientFactory;

    public WeatherController(IMongoDatabase database, IHttpClientFactory httpClientFactory)
    {
        _weatherCollection = database.GetCollection<WeatherData>("weather");
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("{latitude}/{longitude}")]
    public async Task<IActionResult> GetWeather(double latitude, double longitude)
    {
        var apiKey = ""; // сюда ключ api 
        var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric";

        var httpClient = _httpClientFactory.CreateClient();
        var weatherResponse = await httpClient.GetFromJsonAsync<WeatherResponse>(url);

        if (weatherResponse == null)
        {
            return NotFound("Weather data not available.");
        }

        var weatherData = new WeatherData
        {
            Latitude = latitude,
            Longitude = longitude,
            Temperature = weatherResponse.Main.Temp,
            Description = weatherResponse.Weather[0].Description,
            Timestamp = DateTime.UtcNow
        };

        await _weatherCollection.InsertOneAsync(weatherData);

        return Ok(weatherData);
    }
}
