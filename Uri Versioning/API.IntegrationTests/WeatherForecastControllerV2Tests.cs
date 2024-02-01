using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace API.IntegrationTests;

public class WeatherForecastControllerV2Tests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public WeatherForecastControllerV2Tests(WebApplicationFactory<Program> factory)
    {
        const string serviceUrl = "https://localhost:7104/";
        _httpClient = factory.CreateClient();
        _httpClient.BaseAddress = new Uri(serviceUrl);
    }

    [Fact]
    public async Task WhenGetCalled_ShouldReturnTenWeatherForecasts()
    {
        // Arrange

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync("api/v2.0/WeatherForecast");
        string json = await response.Content.ReadAsStringAsync();
        JArray strings = JArray.Parse(json);
        
        // Assert
        strings.Count.Should().Be(10);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}