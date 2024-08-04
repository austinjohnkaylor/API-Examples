using System.Net;
using System.Text.Json.Nodes;
using API.Examples.OData.QueryOptionsApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    /// When querying the Get endpoint on the Customers controller using the $select query option with the Name property, the response should only contain the Name property of each Customer
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.odata.odataoptions.select?view=odata-aspnetcore-8.0</remarks>
    [Fact]
    public async Task Get_SelectQueryOption_WhenQueryingByName_ShouldReturnOnlyName()
    {
        // Arrange
        const string requestUri = "/odata/Customers?$select=Name";
        string expectedResponse = await File.ReadAllTextAsync("../../../ExpectedResponses/Get_SelectQueryOption_WhenQueryingByName_ShouldReturnOnlyName.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// When querying the Get endpoint on the Customers controller using the $expand query option to expand the Orders property, the response should contain the Orders property of each Customer
    /// </summary>
    [Fact]
    public async Task Get_ExpandQueryOption_WhenExpandingCustomersToTheirOrders_ShouldReturnCustomersAndTheirOrders()
    {
        // Arrange
        const string requestUri = "/odata/Customers?$expand=Orders";
        string expectedResponse = await File.ReadAllTextAsync("../../../ExpectedResponses/Get_ExpandQueryOption_WhenExpandingCustomersToTheirOrders_ShouldReturnCustomersAndTheirOrders.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
}