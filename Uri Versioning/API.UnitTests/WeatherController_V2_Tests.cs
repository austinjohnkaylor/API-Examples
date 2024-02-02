﻿using API.Controllers.V2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.UnitTests;

public class WeatherControllerV2Tests
{
    private readonly Mock<ILogger<WeatherForecastController>> _loggerMock = new();

    [Fact]
    public void WhenGetEndpointCalled_ShouldReturnTenWeatherForecasts()
    {
        // Arrange 
        var controller = new WeatherForecastController(_loggerMock.Object);
        // Act
        var result = controller.Get();
        // Assert
        result.Should().HaveCount(10);
    }
}