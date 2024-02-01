using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace API.IntegrationTests;

public class WeatherForecastControllerV1Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public WeatherForecastControllerV1Tests(WebApplicationFactory<Program> factory)
    {
        const string serviceUrl = "https://localhost:7104/";
        _httpClient = factory.CreateClient();
        _httpClient.BaseAddress = new Uri(serviceUrl);
    }

    [Fact]
    public async Task WhenGetCalled_ShouldReturnFiveWeatherForecasts()
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("api/v1.0/WeatherForecast");
        string json = await response.Content.ReadAsStringAsync();
        JArray strings = JArray.Parse(json);
        
        // Assert
        strings.Count.Should().Be(5);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}