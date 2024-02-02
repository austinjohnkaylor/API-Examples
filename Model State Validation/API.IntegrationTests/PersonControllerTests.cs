using System.Net;
using System.Text.Json;
using API.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace API.IntegrationTests;

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
}