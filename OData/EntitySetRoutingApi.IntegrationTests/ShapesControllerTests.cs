using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using EntitySetRoutingApi.Controllers;
using EntitySetRoutingApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EntitySetRoutingApi.IntegrationTests;

/// <summary>
/// Integration test cases for the <see cref="ShapesController"/>
/// </summary>
public class ShapesControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _httpClient;

    public ShapesControllerTests(WebApplicationFactory<Program> application)
    {
        _httpClient = application.CreateClient();
        ResetShapes();
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
    
    /// <summary>
    /// The following PATCH request patches shape 1 (a circle) and shape 2 (a rectangle):
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#patching-a-collection-of-entities</remarks>
    /// <returns>204 No Content if the Patch is successful</returns>
    [Fact]
    public async Task Patching_a_collection_of_entities()
    {
        /*
         * The route template for this request is: PATCH ~/{entityset}
         */
        // Arrange
        const string requestUri = "odata/Shapes";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Patching_a_collection_of_entities.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        /*
         * For the above request to be conventionally-routed, a controller action named Patch (or PatchShapes) that accepts a parameter of type DeltaSet<Shape> decorated with FromBody attribute is expected
         */
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entityset-routing?tabs=net60%2Cvisual-studio#patching-a-collection-of-derived-entities</remarks>
    /// <returns>The following PATCH request patches the rectangles with ID value of 1 and 3 respectively:</returns>
    [Fact]
    public async Task Patching_a_collection_of_derived_entities()
    {
        /*
         * The route template for this request is: PATCH ~/{entityset}/{cast}
         */
        // Arrange
        const string requestUri = "odata/Shapes/EntitySetRoutingApi.Models.Rectangle";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Patching_a_collection_of_derived_entities.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    /// <summary>
    /// The following request returns a single entity with the key value of 1:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#retrieving-a-single-entity</remarks>
    /// <returns></returns>
    [Fact]
    public async Task Retrieving_a_single_entity()
    {
        /*
         * The route templates for this request are:
            GET ~/{entityset}({key})
            GET ~/{entityset}/{key}
         */
        // Arrange
        const string requestUri = "odata/Shapes(1)";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../ExpectedResponses/Retrieving_a_single_entity.json");
        
        /*
         * For the above request to be conventionally-routed, a controller action named Get (or GetShape) that accepts the key parameter is expected
         */
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        
        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following request returns a single Circle derived entity with the key value of 2:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#retrieving-a-single-derived-entity</remarks>
    /// <returns></returns>
    [Fact]
    public async Task Retrieving_a_single_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Shapes(2)/EntitySetRoutingApi.Models.Circle";
        string expectedResponse =
            await File.ReadAllTextAsync("../../../ExpectedResponses/Retrieving_a_single_derived_entity.json");
        /*
         * For the above request to be conventionally-routed, a controller action named GetCircle that accepts the key parameter is expected:
         */
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        
        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following request updates a single rectangle entity with the key value of 1
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#updating-a-single-entity</remarks>
    /// <returns></returns>
    [Fact]
    public async Task Updating_a_single_entity()
    {
        /*
         * The route templates for this request are:
                PUT ~/{entityset}({key})
                PUT ~/{entityset}/{key}
         */
        // Arrange
        const string requestUri = "odata/Shapes(1)";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Updating_a_single_entity.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PutAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    /// <summary>
    /// The following PUT request updates a Circle derived entity with the key value of 2:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#updating-a-single-derived-entity</remarks>
    /// <returns></returns>
    [Fact]
    public async Task Updating_a_single_derived_entity()
    {
        // The route templates for this request are:
        //      PUT ~/{entityset}({key})/{cast}
        //      PUT ~/{entityset}/{key}/{cast}
        // Arrange
        const string requestUri = "odata/Shapes(2)/EntitySetRoutingApi.Models.Circle";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Updating_a_single_derived_entity.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PutAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    /// <summary>
    /// The following PATCH request patches a Rectangle entity with the key value of 3:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#patching-a-single-entity</remarks>
    [Fact]
    public async Task Patching_a_single_entity()
    {
        // Arrange
        const string requestUri = "odata/Shapes(3)";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Patching a single entity.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    /// <summary>
    /// The following PATCH request patches a Circle derived entity with the key value of 2
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#patching-a-single-derived-entity</remarks>
    [Fact]
    public async Task Patching_a_single_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Shapes(2)/EntitySetRoutingApi.Models.Circle";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../RequestBodies/Patching a single derived entity.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    /// <summary>
    /// The following request deletes a Shape entity with the key value of 3:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#deleting-a-single-entity</remarks>
    [Fact]
    public async Task Deleting_a_single_entity()
    {
        // Arrange
        const string requestUri = "odata/Shapes(3)";
        
        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    /// <summary>
    /// The following request deletes a Circle derived entity with the key value of 2:
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#deleting-a-single-derived-entity</remarks>
    [Fact]
    public async Task Deleting_a_single_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Shapes(2)/EntitySetRoutingApi.Models.Circle";
        
        // Act
        HttpResponseMessage response = await _httpClient.DeleteAsync(requestUri);
        
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    
    #region Not Tests

    [ExcludeFromCodeCoverage]
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        ResetShapes();
    }
    
    [ExcludeFromCodeCoverage]
    private static void ResetShapes()
    {
        ShapesController.Shapes.Clear();
        ShapesController.Shapes.AddRange(new List<Shape>
        {
            new Rectangle { Id = 1, Length = 7, Width = 4, Area = 28 },
            new Circle { Id = 2, Radius = 3.5, Area = 38.5 },
            new Rectangle { Id = 3, Length = 8, Width = 5, Area = 40 }
        });
    }

    #endregion
}