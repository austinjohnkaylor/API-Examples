using System.Net.Http.Json;
using InMemoryCaching.API.EntityFramework;
using Microsoft.AspNetCore.Mvc.Testing;

namespace InMemoryCaching.API.IntegrationTests.PersonController;

[Collection("Sequential")]
public class GetPeopleTests(WebApplicationFactory<Program> factory)
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
}