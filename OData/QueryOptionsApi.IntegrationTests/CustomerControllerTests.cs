using Microsoft.AspNetCore.Mvc.Testing;

namespace QueryOptionsApi.IntegrationTests;

/// <summary>
/// Integration Tests for the <see cref="API.Examples.OData.QueryOptionsApi.Controllers.CustomersController"/>
/// </summary>
public class CustomerControllerTests
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _application;

    public CustomerControllerTests()
    {
        _application = new WebApplicationFactory<Program>();
        _httpClient = _application.CreateClient();
    }

    /// <summary>
    /// When calling GET /odata/Customers?$select=Name, the response should contain only the Name property of each Customer
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.odata.odataoptions.select?view=odata-aspnetcore-8.0</remarks>
    [Fact]
    public async Task Get_SelectName()
    {
        // Arrange
        const string requestUri = "/odata/Customers?$select=Name";
        string expectedResponse = await File.ReadAllTextAsync("../../../ExpectedResponses/Get_SelectName.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// When calling GET /odata/Customers?$expand=Orders, the response should contain the Orders property of each Customer
    /// </summary>
    [Fact]
    public async Task Get_ExpandOrders()
    {
        // Arrange
        const string requestUri = "/odata/Customers?$expand=Orders";
        string expectedResponse = await File.ReadAllTextAsync("../../../ExpectedResponses/Get_ExpandOrders.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
    
    /// <summary>
    /// When calling GET /odata/Customers?$select=Name&$expand=Orders($filter=Id gt 1004), the response should contain only the Name property of each Customer and the Orders property of each Customer should only contain Orders with an Id greater than 1004
    /// </summary>
    [Fact]
    public async Task Get_SelectName_ExpandOrders_FilterIdGreaterThan1004_OrderByNameDescending()
    {
        // Arrange
        const string requestUri = "odata/Customers?$select=Name&$expand=Orders($filter=Id gt 1004)&$orderby=Name desc";
        string expectedResponse = await File.ReadAllTextAsync("../../../ExpectedResponses/Get_SelectName_ExpandOrders_FilterIdGreaterThan1004_OrderByNameDescending.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
}