using System.Net;
using System.Net.Http.Json;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InMemoryCaching.API.IntegrationTests.PersonController;

/// <summary>
/// Consider doing to the following to implement segregation of test classes: https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database
/// </summary>
/// <param name="factory"></param>
[Collection("Sequential")]
public class PostPersonTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
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

    [Fact]
    public async Task PostPerson_ShouldReturnConflict()
    {
        // Arrange
        HttpClient client = factory.CreateClient();
        Person person = new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Age = 100,
            Gender = Gender.Male
        };

        // Act
        HttpResponseMessage response = await client.PostAsJsonAsync("/api/person", person);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}