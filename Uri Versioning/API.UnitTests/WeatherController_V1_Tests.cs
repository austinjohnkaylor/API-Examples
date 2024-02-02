using API.Controllers.V1;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace UriVersioning.API.UnitTests;

public class WeatherControllerV1Tests
{
    private readonly Mock<ILogger<WeatherForecastController>> _loggerMock = new();

    [Fact]
    public void WhenGetEndpointCalled_ShouldReturnFiveWeatherForecasts()
    {
        // Arrange 
        var controller = new WeatherForecastController(_loggerMock.Object);
        // Act
        var result = controller.Get();
        // Assert
        result.Should().HaveCount(5);
    }
}