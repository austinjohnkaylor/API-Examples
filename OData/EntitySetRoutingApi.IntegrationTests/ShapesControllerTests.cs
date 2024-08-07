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

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#retrieving-an-entity-set</remarks>
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

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#retrieving-the-count-of-an-entity-set</remarks>
    [Fact]
    public async Task Retrieving_the_count_of_an_entity_set()
    {
        // Arrange
        const string requestUri = "odata/Shapes/$count";
        const int expectedResponse = 3;
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse, Convert.ToInt32(actualResponse));
    }
    
}