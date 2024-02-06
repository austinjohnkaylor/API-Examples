using FluentAssertions;
using InMemoryCaching.API.Controllers;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace InMemoryCaching.API.UnitTests;

public class PersonControllerTests
{
    private readonly Mock<PersonContext> _mockDbContext;
    private readonly Mock<ILogger<PersonController>> _mockLogger;
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly PersonController _personController;

    public PersonControllerTests()
    {
        _mockDbContext = new Mock<PersonContext>();
        _mockLogger = new Mock<ILogger<PersonController>>();
        _mockMemoryCache = new Mock<IMemoryCache>();
        _personController = new PersonController(_mockDbContext.Object, _mockMemoryCache.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task WhenPeopleExistInCache_ThenReturnOkResult()
    {
        // Arrange
        const string cacheKey = "PeopleList";
        // https://github.com/MichalJankowskii/Moq.EntityFrameworkCore
        PersonGenerator.Generate(100);
        IList<Person> people = PersonGenerator.People;
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(people);
        
        MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
        var cacheEntry = new Mock<ICacheEntry>();
        _mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(cacheEntry.Object);
        //_mockMemoryCache.Setup(cache => cache.TryGetValue(cacheKey, out people));
        
        // Act
        var result = await _personController.GetPeople();
        var resultHttp = result.Result as OkObjectResult;
        var resultPeople = resultHttp.Value as List<Person>;
        
        // Assert
        resultHttp.Should().BeOfType<OkObjectResult>();
        resultHttp.StatusCode.Should().Be(200);
        resultPeople.Count.Should().Be(people.Count);
    }
    
}