using API.Examples.OData.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Prog = API.Examples.OData.NavigationRoutingApi.Program;

namespace API.Examples.OData.IntegrationTests.NavigationRoutingApi;

/// <summary>
/// Integration tests for the <see cref="API.Examples.OData.NavigationRoutingApi.Controllers.EmployeesController"/>
/// </summary>
public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Prog>>, IDisposable
{
    private readonly HttpClient _httpClient = new WebApplicationFactory<Prog>().CreateClient();

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#retrieving-a-single-valued-or-collection-valued-navigation-property-on-an-entity</remarks>
    [Fact]
    public async Task Retrieving_a_single_valued_or_collection_valued_navigation_property_on_an_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(1)/Supervisor";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../NavigationRoutingApi/ExpectedResponses/Retrieving a single-valued or collection-valued navigation property on an entity.json");
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }


    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }
}