using API.Controllers.V3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace UriVersioning.API.UnitTests;

public class WeatherControllerV3Tests
{
    private readonly Mock<ILogger<WeatherForecastController>> _loggerMock = new();

    [Fact]
    public void WhenGetEndpointCalled_ShouldReturnFiftyWeatherForecasts()
    {
        // Arrange 
        var controller = new WeatherForecastController(_loggerMock.Object);
        // Act
        var result = controller.Get();
        // Assert
        result.Should().HaveCount(50);
    }
}