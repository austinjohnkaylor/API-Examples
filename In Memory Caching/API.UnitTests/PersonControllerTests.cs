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
    private const string PeopleCacheKey = "ListOfPeople";
    private const string PersonCacheKey = "Person_{0}"; // {0} is the employee id
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
        _cache.Set(PeopleCacheKey, _people);
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

    [Fact]
    public async Task GetPerson_ReturnsPersonFromCache_WhenPersonExistsInCache()
    {
        // Arrange
        var id = 1;
        Person personToGet = _people.First(person => person.Id == id);
        _cache.Set(string.Format(PersonCacheKey, id), personToGet);
        // Act
        var result = await _personController.GetPerson(id);
        OkObjectResult? resultHttp = result.Result as OkObjectResult;
        Person? returnedPerson = resultHttp?.Value as Person;

        // Assert
        resultHttp.Should().BeOfType<OkObjectResult>();
        resultHttp?.StatusCode.Should().Be(200);
        returnedPerson.Should().BeEquivalentTo(personToGet);
    }

    [Fact]
    public async Task GetPerson_ReturnsPersonFromDatabase_WhenPersonDoesNotExistInCache()
    {
        // Arrange
        const int id = 1;
        Person personToGet = _people.First(person => person.Id == id);
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        // Act
        var result = await _personController.GetPerson(id);
        OkObjectResult? resultHttp = result.Result as OkObjectResult;
        Person? returnedPerson = resultHttp?.Value as Person;

        // Assert
        resultHttp.Should().BeOfType<OkObjectResult>();
        resultHttp?.StatusCode.Should().Be(200);
        returnedPerson.Should().BeEquivalentTo(personToGet);
    }

    [Fact]
    public async Task GetPerson_ReturnsNotFound_WhenPersonDoesNotExistInCacheOrDatabase()
    {
        // Arrange
        const int id = 1001; // outside the bounds of the 1000 people populated in the database
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        // Act
        var result = await _personController.GetPerson(id);
        NotFoundResult? resultHttp = result.Result as NotFoundResult;

        // Assert
        resultHttp.Should().BeOfType<NotFoundResult>();
        resultHttp?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetPerson_SavesPersonFromDatabaseToCache_WhenPersonDoesNotExistInCache()
    {
        // Arrange
        const int id = 1;
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);

        // Act
        var result = await _personController.GetPerson(id);
        Person? personFromDatabaseSavedToCache = _cache.Get<Person>(string.Format(PersonCacheKey, id));
        OkObjectResult? resultHttp = result.Result as OkObjectResult;
        Person? returnedPerson = resultHttp?.Value as Person;

        // Assert
        resultHttp.Should().BeOfType<OkObjectResult>();
        resultHttp?.StatusCode.Should().Be(200);
        returnedPerson.Should().BeEquivalentTo(personFromDatabaseSavedToCache);
    }

    [Fact]
    public async Task PutPerson_ReturnsBadRequest_WhenIdEnteredDoesNotMatchPersonsId()
    {
        // Arrange
        const int id = 1;
        Person person = _people.Find(a => a.Id == 2) ?? new Person { Id = 2 };
        // Act
        IActionResult result = await _personController.PutPerson(id, person);
        BadRequestResult? resultHttp = result as BadRequestResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(BadRequestResult));
        resultHttp?.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task PutPerson_ReturnsNotFound_WhenIdEnteredDoesNotMatchExistingPersonInDatabase()
    {
        // Arrange
        const int id = 1001; // outside the bounds of the 1000 people populated in the database
        Person? person = new() { Id = id };
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        // Act
        IActionResult result = await _personController.PutPerson(id, person);
        NotFoundResult? resultHttp = result as NotFoundResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(NotFoundResult));
        resultHttp?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task PutPerson_ReturnsNoContent_WhenIdMatchesPersonsIdAndPersonExistsInDatabase()
    {
        // Arrange
        const int id = 5; // outside the bounds of the 1000 people populated in the database
        Person? person = _people.FirstOrDefault(person => person.Id == id);
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(c => c.SetModified(It.IsAny<Person>()));
        //_mockDbContext.Setup(context => context.Entry(originalPerson).State).Returns(EntityState.Modified);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        IActionResult result = await _personController.PutPerson(id, person);
        NoContentResult? resultHttp = result as NoContentResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(NoContentResult));
        resultHttp?.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task PutPerson_RemovesPersonFromCache_WhenIdMatchesPersonsIdAndPersonExistsInDatabase()
    {
        // Arrange
        const int id = 5; // outside the bounds of the 1000 people populated in the database
        Person? person = _people.FirstOrDefault(person => person.Id == id);
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(c => c.SetModified(It.IsAny<Person>()));
        //_mockDbContext.Setup(context => context.Entry(originalPerson).State).Returns(EntityState.Modified);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        await _personController.PutPerson(id, person);
        // Assert
        _cache.Get<Person>(string.Format(PersonCacheKey, id)).Should().BeNull();
    }

    [Fact]
    public async Task PostPerson_ReturnsConflict_WhenPersonExistsInDatabase()
    {
        // Arrange
        Person person = _people.First();
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        // Act
        IActionResult result = await _personController.PostPerson(person);
        ObjectResult? resultHttp = result as ObjectResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(ObjectResult));
        resultHttp?.StatusCode.Should().Be(409);
        resultHttp.Value.Should().BeOfType<Person>();
    }

    [Fact]
    public async Task PostPerson_ReturnsOk_WhenPersonDoesNotExistInDatabase()
    {
        // Arrange
        Person person = new() { Id = 1001 };
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        IActionResult result = await _personController.PostPerson(person);
        CreatedAtActionResult? resultHttp = result as CreatedAtActionResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(CreatedAtActionResult));
        resultHttp?.StatusCode.Should().Be(201);
        resultHttp.Value.Should().BeOfType<Person>();
    }

    [Fact]
    public async Task PostPerson_InvalidatesPeopleCache_WhenPersonIsAddedToDatabase()
    {
        // Arrange
        Person person = new() { Id = 1001 };
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        await _personController.PostPerson(person);
        // Assert
        _cache.Get<List<Person>>(PeopleCacheKey).Should().BeNull();
    }

    [Fact]
    public async Task PostPerson_AddsPersonToCache_WhenPersonIsAddedToDatabase()
    {
        // Arrange
        Person person = new() { Id = 1001 };
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        await _personController.PostPerson(person);
        // Assert
        _cache.Get<Person>(string.Format(PersonCacheKey, person.Id)).Should().Be(person);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNotFound_WhenPersonDoesNotExistInDatabase()
    {
        // Arrange
        const int id = 1001; // outside the bounds of the 1000 people populated in the database
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        // Act
        IActionResult result = await _personController.DeletePerson(id);
        NotFoundResult? resultHttp = result as NotFoundResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(NotFoundResult));
        resultHttp?.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task DeletePerson_ReturnsNoContent_WhenPersonExistsInDatabase()
    {
        // Arrange
        const int id = 5; // outside the bounds of the 1000 people populated in the database
        Person? person = _people.FirstOrDefault(person => person.Id == id);
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(context => context.People.FindAsync(id)).ReturnsAsync(person);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        IActionResult result = await _personController.DeletePerson(id);
        NoContentResult? resultHttp = result as NoContentResult;
        // Assert
        resultHttp.Should().BeOfType(typeof(NoContentResult));
        resultHttp?.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeletePerson_RemovesPersonFromCache_WhenPersonExistsInDatabase()
    {
        // Arrange
        const int id = 5; // outside the bounds of the 1000 people populated in the database
        Person? person = _people.FirstOrDefault(person => person.Id == id);
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(context => context.People.FindAsync(id)).ReturnsAsync(person);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        await _personController.DeletePerson(id);
        // Assert
        _cache.Get<Person>(string.Format(PersonCacheKey, id)).Should().BeNull();
    }

    [Fact]
    public async Task DeletePerson_InvalidatesPeopleCache_WhenPersonIsDeletedFromDatabase()
    {
        // Arrange
        const int id = 5; // outside the bounds of the 1000 people populated in the database
        Person? person = _people.FirstOrDefault(person => person.Id == id);
        _mockDbContext.Setup(x => x.People).ReturnsDbSet(_people);
        _mockDbContext.Setup(context => context.People.FindAsync(id)).ReturnsAsync(person);
        _mockDbContext.Setup(context => context.SaveChangesAsync(new CancellationToken())).ReturnsAsync(1);
        // Act
        await _personController.DeletePerson(id);
        // Assert
        _cache.Get<List<Person>>(PeopleCacheKey).Should().BeNull();
    }
}