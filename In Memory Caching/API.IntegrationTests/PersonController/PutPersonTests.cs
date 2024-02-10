using System.Net;
using System.Net.Http.Json;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InMemoryCaching.API.IntegrationTests.PersonController;

[Collection("Sequential")]
public class PutPersonTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task PutPerson_ShouldReturnNoContent()
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
        HttpResponseMessage response = await client.PutAsJsonAsync("/api/person/1", person);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task PutPerson_ShouldReturnBadRequest()
    {
        // Arrange
        HttpClient client = factory.CreateClient();
        Person person = new()
        {
            Id = 2,
            FirstName = "John",
            LastName = "Doe",
            Age = 100,
            Gender = Gender.Female
        };
        var id = 1;
        
        // Act
        HttpResponseMessage response = await client.PutAsJsonAsync($"/api/person/{id}", person);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PutPerson_ShouldReturnNotFound()
    {
        // Arrange
        HttpClient client = factory.CreateClient();
        Person person = new()
        {
            Id = 1111,
            FirstName = "John",
            LastName = "Doe",
            Age = 100,
            Gender = Gender.Female
        };
        
        // Act
        HttpResponseMessage response = await client.PutAsJsonAsync("/api/person/1111", person);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}