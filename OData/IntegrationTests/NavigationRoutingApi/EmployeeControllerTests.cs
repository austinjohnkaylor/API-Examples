using System.Diagnostics;
using System.Net;
using API.Examples.OData.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Http;
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
    /// The following request returns the Supervisor single-valued navigation property on employee 1
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

    /// <summary>
    /// The following request returns the DirectReports collection-valued navigation property on employee 5 (a manager)
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#retrieving-a-single-valued-or-collection-valued-navigation-property-on-a-derived-entity</remarks>
    [Fact]
    public async Task Retrieving_a_single_valued_or_collection_valued_navigation_property_on_a_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(5)/API.Examples.OData.NavigationRoutingApi.Models.Manager/DirectReports";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../NavigationRoutingApi/ExpectedResponses/Retrieving a single-valued or collection-valued navigation property on a derived entity.json");
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following request returns the number of items in the DirectReports collection-valued navigation property on employee 5 (a manager)
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#retrieving-the-count-of-a-collection-valued-navigation-property-on-a-derived-entity</remarks>
    [Fact]
    public async Task Retrieving_the_count_of_a_collection_valued_navigation_property_on_a_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(5)/API.Examples.OData.NavigationRoutingApi.Models.Manager/DirectReports/$count";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../NavigationRoutingApi/ExpectedResponses/Retrieving the count of a collection-valued navigation property on a derived entity.json");
        // HttpContent requestBody =
        //     new StringContent(
        //         await File.ReadAllTextAsync("../../../EntitySetRoutingApi/RequestBodies/Retrieving the count of a collection-valued navigation property on a derived entity\n.json"));
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following POST request adds an employee to the Peers collection-valued navigation property on employee 4
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#post-to-a-collection-valued-navigation-property-on-an-entity</remarks>
    [Fact]
    public async Task Post_to_a_collection_valued_navigation_property_on_an_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(4)/Peers";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../NavigationRoutingApi/RequestBodies/Post to a collection-valued navigation property on an entity.json"));
        string expectedResponse =
            await File.ReadAllTextAsync("../../../NavigationRoutingApi/ExpectedResponses/Post to a collection-valued navigation property on an entity.json");
        Debug.Assert(requestBody.Headers.ContentType != null, "requestBody.Headers.ContentType != null");
        requestBody.Headers.ContentType.MediaType = "application/json";
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal((HttpStatusCode)StatusCodes.Status201Created, response.StatusCode);
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following POST request adds an employee to the DirectReports collection-valued navigation property on employee 6 (a manager):
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#post-to-a-collection-valued-navigation-property-on-a-derived-entity</remarks>
    [Fact]
    public async Task Post_to_a_collection_valued_navigation_property_on_a_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(5)/API.Examples.OData.NavigationRoutingApi.Models.Manager/DirectReports";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../NavigationRoutingApi/RequestBodies/Post to a collection-valued navigation property on a derived entity.json"));
        string expectedResponse =
            await File.ReadAllTextAsync("../../../NavigationRoutingApi/ExpectedResponses/Post to a collection-valued navigation property on a derived entity.json");
        Debug.Assert(requestBody.Headers.ContentType != null, "requestBody.Headers.ContentType != null");
        requestBody.Headers.ContentType.MediaType = "application/json";
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal((HttpStatusCode)StatusCodes.Status201Created, response.StatusCode);
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
    
    /// <summary>
    /// The following PUT request updates a Supervisor single-valued navigation property on employee 1
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#updating-a-single-valued-navigation-property-on-an-entity</remarks>
    [Fact]
    public async Task Updating_a_single_valued_navigation_property_on_an_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(1)/Supervisor";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../NavigationRoutingApi/RequestBodies/Updating a single-valued navigation property on an entity.json"));
        Debug.Assert(requestBody.Headers.ContentType != null, "requestBody.Headers.ContentType != null");
        requestBody.Headers.ContentType.MediaType = "application/json";
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal((HttpStatusCode)StatusCodes.Status200OK, response.StatusCode);
    }

    /// <summary>
    /// The following PUT request updates a PersonalAssistant single-valued navigation property on employee 6 (a manager)
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#updating-a-single-valued-navigation-property-on-a-derived-entity</remarks>
    [Fact]
    public async Task Updating_a_single_valued_navigation_property_on_a_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(6)/API.Examples.OData.NavigationRoutingApi.Models.Manager/PersonalAssistant";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../NavigationRoutingApi/RequestBodies/Updating a single-valued navigation property on a derived entity.json"));
        Debug.Assert(requestBody.Headers.ContentType != null, "requestBody.Headers.ContentType != null");
        requestBody.Headers.ContentType.MediaType = "application/json";
        // Act
        HttpResponseMessage response = await _httpClient.PutAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal((HttpStatusCode)StatusCodes.Status200OK, response.StatusCode);
    }
    
    /// <summary>
    /// The following PATCH request patches a Supervisor navigation property on employee 1
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#patching-a-navigation-property-on-an-entity</remarks>
    [Fact]
    public async Task Patching_a_navigation_property_on_an_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(1)/Supervisor";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../NavigationRoutingApi/RequestBodies/Patching a navigation property on an entity.json"));
        Debug.Assert(requestBody.Headers.ContentType != null, "requestBody.Headers.ContentType != null");
        requestBody.Headers.ContentType.MediaType = "application/json";
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal((HttpStatusCode)StatusCodes.Status200OK, response.StatusCode);
    }

    /// <summary>
    /// The following PATCH request patches a PersonalAssistant navigation property on employee 6 (a manager)
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#patching-a-navigation-property-on-a-derived-entity</remarks>
    [Fact]
    public async Task Patching_a_navigation_property_on_a_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(6)/API.Examples.OData.NavigationRoutingApi.Models.Manager/PersonalAssistant";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../NavigationRoutingApi/RequestBodies/Patching a navigation property on a derived entity.json"));
        Debug.Assert(requestBody.Headers.ContentType != null, "requestBody.Headers.ContentType != null");
        requestBody.Headers.ContentType.MediaType = "application/json";
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal((HttpStatusCode)StatusCodes.Status200OK, response.StatusCode);
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }
}