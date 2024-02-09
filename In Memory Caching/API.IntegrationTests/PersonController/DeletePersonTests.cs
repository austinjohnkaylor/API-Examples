using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InMemoryCaching.API.IntegrationTests.PersonController;

[Collection("Sequential")]
public class DeletePersonTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task DeletePerson_ShouldReturnNoContent()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.DeleteAsync("/api/person/1");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePerson_ShouldReturnNotFound()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.DeleteAsync("/api/person/1001");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePerson_ShouldReturnNotFoundWhenIdIsZero()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.DeleteAsync("/api/person/0");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task DeletePerson_ShouldReturnNotFoundWhenPersonIsDeletedTwice()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.DeleteAsync("/api/person/1");
        HttpResponseMessage secondResponse = await client.DeleteAsync("/api/person/1");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, secondResponse.StatusCode);
    }
}