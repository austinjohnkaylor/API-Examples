using System.Net;
using API;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace UriVersioning.API.IntegrationTests;

public class WeatherForecastControllerV3Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public WeatherForecastControllerV3Tests(WebApplicationFactory<Program> factory)
    {
        const string serviceUrl = "https://localhost:7104/";
        _httpClient = factory.CreateClient();
        _httpClient.BaseAddress = new Uri(serviceUrl);
    }

    [Fact]
    public async Task WhenGetCalled_ShouldReturnFiftyWeatherForecasts()
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("api/v3.0/WeatherForecast");
        string json = await response.Content.ReadAsStringAsync();
        JArray strings = JArray.Parse(json);
        
        // Assert
        strings.Count.Should().Be(50);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}