using System.Net;
using API.Controllers;
using API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Moq;

namespace API.UnitTests;

public class PersonControllerTests
{
    private readonly Mock<ILogger<PeopleController>> _loggerMock = new();
    private readonly Mock<IActionFilter> _mockValidationFilter = new();

    public PersonControllerTests()
    {
        _mockValidationFilter.Setup(x => x.OnActionExecuting(It.IsAny<ActionExecutingContext>()))
            .Callback<ActionExecutingContext>(context =>
            {
                if (!context.ModelState.IsValid)
                {
                    context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                }
            });
    }

    [Fact]
    public void WhenGetEndpointIsCalled_WithValidGuid_ShouldReturn200()
    {
        // Arrange 
        var controller = new PeopleController(_loggerMock.Object);
        // Act
        IActionResult result = controller.Get(new Guid());
        var okResult = (OkObjectResult)result;
        // Assert
        result.Should().BeOfType(typeof(OkObjectResult));
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeOfType(typeof(Person));
    }

    [Fact]
    public void WhenPostEndpointIsCalled_WithValidPersonObject_ShouldReturn201()
    {
        // Arrange 
        var controller = new PeopleController(_loggerMock.Object);
        // Act
        IActionResult result = controller.Post(new Person());
        var createdResult = (CreatedAtActionResult)result;
        // Assert
        result.Should().BeOfType(typeof(CreatedAtActionResult));
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeOfType(typeof(Person));
    }
    
    [Fact]
    public void WhenPostEndpointIsCalled_WithFirstNameLongerThan50Characters_ShouldReturn422()
    {
        // Arrange 
        var controller = new PeopleController(_loggerMock.Object);
        Person person = new()
        {
            FirstName = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz",
            LastName = "B",
            Age = 50
        };
        // Act
        IActionResult result = controller.Post(person);
        var unprocessableEntityObjectResult = (UnprocessableEntityObjectResult)result;
        // Assert
        result.Should().BeOfType(typeof(UnprocessableEntityObjectResult));
        unprocessableEntityObjectResult.StatusCode.Should().Be(422);
        unprocessableEntityObjectResult.Value.Should().BeOfType(typeof(Person));
    }
}