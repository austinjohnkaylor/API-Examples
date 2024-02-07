using FluentAssertions;
using InMemoryCaching.API.Controllers;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace InMemoryCaching.API.UnitTests;

public class PersonControllerTests
{
    private readonly Mock<PersonContext> _mockDbContext;
    private readonly Mock<ILogger<PersonController>> _mockLogger;
    private readonly PersonController _personController;
    private List<Person> _people;
    private const string cacheKey = "ListOfPeople";
    private MemoryCache _cache;


    public PersonControllerTests()
    {
        _mockDbContext = new Mock<PersonContext>();
        _mockLogger = new Mock<ILogger<PersonController>>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _personController = new PersonController(_mockDbContext.Object, _cache, _mockLogger.Object);
        PersonGenerator.Generate(100);
        _people = PersonGenerator.People;
    }
    
    /// <summary>
    /// Tests when the cache contains data
    /// </summary>
    /// <returns>All the people from the cache instead of the database</returns>
    [Fact]
    public async Task GetPeople_ReturnsOkResult_WhenCacheContainsData()
    {
        // Arrange
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(new List<Person>());
        _cache.Set(cacheKey, _people);
        // Act
        var result = await _personController.GetPeople();
        OkObjectResult? resultHttp = result.Result as OkObjectResult;
        var returnedPeople = resultHttp?.Value as List<Person>;

        // Assert
        resultHttp.Should().BeOfType<OkObjectResult>();
        resultHttp?.StatusCode.Should().Be(200);
        returnedPeople.Should().Equal(_people);
    }
    
    /// <summary>
    /// Tests when the cache is empty 
    /// </summary>
    /// <returns>All the people from the database instead of the cache</returns>
    [Fact]
    public async Task GetPeople_FetchesFromDatabase_WhenCacheIsEmpty()
    {
        // Arrange
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        // Act
        var result = await _personController.GetPeople();
        OkObjectResult? resultHttp = result.Result as OkObjectResult;
        var returnedPeople = resultHttp?.Value as List<Person>;

        // Assert
        resultHttp.Should().BeOfType<OkObjectResult>();
        resultHttp?.StatusCode.Should().Be(200);
        returnedPeople.Should().Equal(_people);
    }
    
}