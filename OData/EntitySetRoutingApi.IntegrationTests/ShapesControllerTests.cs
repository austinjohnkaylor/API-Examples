using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using EntitySetRoutingApi.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EntitySetRoutingApi.IntegrationTests;

/// <summary>
/// Integration test cases for the <see cref="ShapesController"/>
/// </summary>
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
    /// The following request should return the Shapes entity set - basically a collection of Shape entities:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#retrieving-an-entity-set</remarks>
    /// <returns>The response should contain 3 shape objects - 2 rectangles and 1 circle. Since Rectangle and Circle are derived types, each of the shape objects contain an @odata.type property specifying the type of the entity.</returns>
    [Fact]
    public async Task Retrieving_an_entity_set()
    {
        // Arrange
        const string requestUri = "/odata/Shapes";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../ExpectedResponses/Retrieving an entity set.json");

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following request should return a count of items in the Shapes entity set:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#retrieving-the-count-of-an-entity-set</remarks>
    /// <returns>The response should contain the count of items in the Shapes entity set - 3.</returns>
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

    /// <summary>
    /// The following request should return a collection of derived Rectangle entities
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#retrieving-a-collection-of-derived-entities</remarks>
    /// <returns>The response should contain 2 rectangle objects</returns>
    [Fact]
    public async Task Retrieving_a_collection_of_derived_entities()
    {
        // Arrange
        const string requestUri = "odata/Shapes/EntitySetRoutingApi.Models.Rectangle";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../ExpectedResponses/Retrieving_a_collection_of_derived_entities.json");
        /*
         * For the above request to be conventionally-routed, a controller action named GetFromRectangle (or GetShapesFromRectangle) is expected
         * in the ShapesController. The action should return a collection of Rectangle entities.
         *
         * TODO: Figure out why the controller action needs to be named GetFromRectangle or GetShapesFromRectangle
         */
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following request returns a count of items in the Shapes entity set that are rectangles:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#retrieving-the-count-of-a-collection-of-derived-entities</remarks>
    /// <returns>The response should be 2</returns>
    [Fact]
    public async Task Retrieving_the_count_of_a_collection_of_derived_entities()
    {
        // Arrange
        const string requestUri = "odata/Shapes/EntitySetRoutingApi.Models.Rectangle/$count";
        const int expectedResponse = 2;

        /*
         * For the above request to be conventionally-routed, a controller action named GetFromRectangle (or GetShapesFromRectangle) is expected, same as is expected when retrieving a collection of derived entities.
         * However, the controller action needs to be decorated with EnableQuery attribute:
         */

        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse, Convert.ToInt32(actualResponse));
    }

    /// <summary>
    /// The following POST request adds a Shape entity to the Shapes entity set
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#adding-an-entity-to-an-entity-set</remarks>
    /// <returns>The response status code should be 201</returns>
    [Fact]
    public async void Adding_an_entity_to_an_entity_set()
    {
        // Arrange
        const string requestUri = "odata/Shapes";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../ExpectedResponses/Adding_an_entity_to_an_entity_set.json");
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Adding_an_entity_to_an_entity_set.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        /*
         * For the above request to be conventionally-routed, a controller action named Post (or PostShape)
         * that accepts a parameter of type Shape decorated with FromBody attribute is expected in the ShapesController
         */
        // Act
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri, requestBody);
        
        // Assert
        Debug.Assert(response.Headers.Location != null, "response.Headers.Location != null");
        Assert.Equal("http://localhost/odata/Shapes(4)", response.Headers.Location.ToString());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following POST request adds a Circle entity to the Shapes entity set
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#adding-a-derived-entity-to-an-entity-set</remarks>
    /// <returns>The response status code should be 201</returns>
    [Fact]
    public async Task Adding_a_derived_entity_to_an_entity_set()
    {
        /*
         * To create a derived entity in an entity set, the client sends a POST request to that entity set's URL with the fully-qualified name of the derived type appended at the end.
            The route template for this request is: POST ~/{entityset}/{cast}
         */
        // Arrange
        const string requestUri = "odata/Shapes/EntitySetRoutingApi.Models.Circle";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../ExpectedResponses/Adding_a_derived_entity_to_an_entity_set.json");
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Adding_a_derived_entity_to_an_entity_set.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        /*
         * For the above request to be conventionally-routed, a controller action named PostFromCircle (or PostShapeFromCircle) that accepts a parameter of type Circle decorated with FromBody attribute is expected
         */
        HttpResponseMessage response = await _httpClient.PostAsync(requestUri, requestBody);
        
        // Assert
        Debug.Assert(response.Headers.Location != null, "response.Headers.Location != null");
        Assert.Equal("http://localhost/odata/Shapes(5)", response.Headers.Location.ToString());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

}