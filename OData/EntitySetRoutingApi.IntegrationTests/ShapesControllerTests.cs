using Microsoft.AspNetCore.Mvc.Testing;

namespace EntitySetRoutingApi.IntegrationTests;

public class ShapesControllerTests
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _application;

    public ShapesControllerTests()
    {
        _application = new WebApplicationFactory<Program>();
        _httpClient = _application.CreateClient();
    }

    [Fact]
    public async Task Retrieving_an_entity_set()
    {
        // Arrange
        const string requestUri = "/odata/Shapes";
        string expectedResponse = await File.ReadAllTextAsync("../../../ExpectedResponses/Retrieving an entity set.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    
}