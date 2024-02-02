using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using ModelStateValidation.API.Models;

namespace ModelStateValidation.API.IntegrationTests;

public class PersonControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public PersonControllerTests(WebApplicationFactory<Program> factory)
    {
        const string serviceUrl = "https://localhost:7220/";
        _httpClient = factory.CreateClient();
        _httpClient.BaseAddress = new Uri(serviceUrl);
    }
    
    [Fact]
    public async Task WhenGetEndpointIsCalled_WithValidGuid_ShouldReturn200()
    {
        // Arrange 
        HttpResponseMessage response = await _httpClient.GetAsync($"People/{new Guid()}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task WhenPostEndpointIsCalled_WithValidPerson_ShouldReturn201()
    {
        // Arrange
        Person person = new()
        {
            Age = 100,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male
        };

        // Act
        string json = JsonSerializer.Serialize(person);
        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync("People", content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task WhenPostEndpointIsCalled_WithFirstNameLongerThan50Characters_ShouldReturn422()
    {
        // Arrange
        Person person = new()
        {
            Age = 100,
            FirstName = "lakjdfg;lkja;lkwjelkra;ljwe;lktja;lkejtljaelkjtklaj",
            LastName = "Doe",
            Gender = Gender.Male
        };

        // Act
        string json = JsonSerializer.Serialize(person);
        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync("People", content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableContent);
    }
    
    [Fact]
    public async Task WhenPostEndpointIsCalled_WithAgeGreaterThan130_ShouldReturn422()
    {
        // Arrange
        Person person = new()
        {
            Age = 140,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male
        };

        // Act
        string json = JsonSerializer.Serialize(person);
        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync("People", content);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableContent);
    }
}