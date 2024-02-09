using System.Net;
using System.Net.Http.Json;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InMemoryCaching.API.IntegrationTests;

public class PersonControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetPeople_ShouldReturnPeople()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person");

        // Assert
        response.EnsureSuccessStatusCode();
        var persons = await response.Content.ReadFromJsonAsync<IEnumerable<Person>>();
        Assert.NotNull(persons);
        Assert.Equal(1000, persons.Count());
    }
    
    [Fact]
    public async Task GetPeople_ShouldReturnPeopleFromCache()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person");
        HttpResponseMessage secondResponse = await client.GetAsync("/api/person");


        // Assert
        secondResponse.EnsureSuccessStatusCode();
        var persons = await secondResponse.Content.ReadFromJsonAsync<IEnumerable<Person>>();
        Assert.NotNull(persons);
        Assert.Equal(1000, persons.Count());
    }
    
    [Fact]
    public async Task GetPerson_ShouldReturnPerson()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person/1");

        // Assert
        response.EnsureSuccessStatusCode();
        Person? person = await response.Content.ReadFromJsonAsync<Person>();
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
    }
    
    [Fact]
    public async Task GetPerson_ShouldReturnPersonFromCache()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person/1");
        HttpResponseMessage secondResponse = await client.GetAsync("/api/person/1");

        // Assert
        secondResponse.EnsureSuccessStatusCode();
        Person? person = await secondResponse.Content.ReadFromJsonAsync<Person>();
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
    }
    
    [Fact]
    public async Task GetPerson_ShouldReturnNotFound()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person/1001");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetPerson_ShouldReturnNotFoundWhenIdIsZero()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person/0");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task PostPerson_ShouldReturnCreated()
    {
        // Arrange
        HttpClient client = factory.CreateClient();
        Person person = new()
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 100,
            Gender = Gender.Male
        };

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/person", person);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}