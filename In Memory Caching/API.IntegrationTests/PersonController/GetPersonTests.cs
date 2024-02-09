using System.Net;
using System.Net.Http.Json;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InMemoryCaching.API.IntegrationTests.PersonController;

[Collection("Sequential")]
public class GetPersonTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GetPerson_ShouldReturnPerson()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person/10");

        // Assert
        response.EnsureSuccessStatusCode();
        Person? person = await response.Content.ReadFromJsonAsync<Person>();
        Assert.NotNull(person);
        Assert.Equal(10, person.Id);
    }
    
    [Fact]
    public async Task GetPerson_ShouldReturnPersonFromCache()
    {
        // Arrange
        HttpClient client = factory.CreateClient();

        // Act
        HttpResponseMessage response = await client.GetAsync("/api/person/11");
        HttpResponseMessage secondResponse = await client.GetAsync("/api/person/11");

        // Assert
        secondResponse.EnsureSuccessStatusCode();
        Person? person = await secondResponse.Content.ReadFromJsonAsync<Person>();
        Assert.NotNull(person);
        Assert.Equal(11, person.Id);
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
}