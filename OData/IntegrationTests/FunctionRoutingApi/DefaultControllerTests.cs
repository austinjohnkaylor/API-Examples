using Microsoft.AspNetCore.Mvc.Testing;
using API.Examples.OData.FunctionRoutingApi;
using API.Examples.OData.IntegrationTests.Extensions;

namespace API.Examples.OData.IntegrationTests.FunctionRoutingApi;

public class DefaultControllerTests(WebApplicationFactory<Program> applicationFactory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient = applicationFactory.CreateClient();

    /// <summary>
    /// The following request invokes the <see cref="API.Examples.OData.FunctionRoutingApi.DefaultController.GetSalary"/> unbound function
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#routing-unbound-edm-functions</remarks>
    [Fact]
    public async Task Routing_unbound_Edm_functions()
    {
        // Arrange
        const string requestUri = "odata/GetSalary(hourlyRate=17,hoursWorked=40)";
        string expectedResponse = await File.ReadAllTextAsync("../../../FunctionRoutingApi/ExpectedResponses/Routing_unbound_Edm_functions.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
}