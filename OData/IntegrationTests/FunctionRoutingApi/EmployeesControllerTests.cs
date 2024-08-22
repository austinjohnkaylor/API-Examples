using System.Diagnostics.CodeAnalysis;
using API.Examples.OData.FunctionRoutingApi;
using API.Examples.OData.FunctionRoutingApi.Models;
using API.Examples.OData.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace API.Examples.OData.IntegrationTests.FunctionRoutingApi;

public class EmployeesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public EmployeesControllerTests(WebApplicationFactory<Program> applicationFactory)
    {
        _httpClient = applicationFactory.CreateClient();
        ResetEmployees();
    }

    [ExcludeFromCodeCoverage]
    private static void ResetEmployees()
    {
        EmployeesController.Employees.Clear();
        EmployeesController.Employees = [
            new Employee { Id = 1, Name = "Employee 1", PerfRating = 8 },
            new Employee { Id = 2, Name = "Employee 2", PerfRating = 7 },
            new Employee { Id = 3, Name = "Employee 3", PerfRating = 5 },
            new Employee { Id = 4, Name = "Employee 4", PerfRating = 3 },
            new Manager { Id = 5, Name = "Employee 5", PerfRating = 7, Bonus = 2900 },
            new Manager { Id = 6, Name = "Employee 6", PerfRating = 9, Bonus = 3700 }
        ];
    }

    /// <summary>
    /// The following request invokes the GetHighestRating function bound to the Employees entity set. The URL for the function is the function name appended to the entity set's URL
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-an-entity-set-or-singleton</remarks>
    [Fact]
    public async Task Invoking_a_function_bound_to_an_entity_set_or_singleton()
    {
        // Arrange
        const string requestUri = "odata/Employees/GetHighestRating()";
        string expectedResponse = await File.ReadAllTextAsync("../../../FunctionRoutingApi/ExpectedResponses/Invoking_a_function_bound_to_an_entity_set_or_singleton.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
    
    /// <summary>
    /// The following request invokes the GetRating function bound to employee 1. The URL for the function is the function name appended to the entity's URL
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-an-entity</remarks>
    [Fact]
    public async Task Invoking_a_function_bound_to_an_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(1)/GetRating()";
        string expectedResponse = await File.ReadAllTextAsync("../../../FunctionRoutingApi/ExpectedResponses/Invoking_a_function_bound_to_an_entity.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
    
    /// <summary>
    /// The following request invokes the GetHighestBonus function bound to the collection of Manager derived entities. The URL for the function is the function name appended to the collection of derived entities' URL
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-a-collection-of-derived-entities-or-derived-singleton</remarks>
    [Fact]
    public async Task Invoking_a_function_bound_to_a_collection_of_derived_entities_or_derived_singleton()
    {
        // Arrange
        const string requestUri = "odata/Employees/API.Examples.OData.FunctionRoutingApi.Models.Manager/GetHighestBonus()";
        string expectedResponse = await File.ReadAllTextAsync("../../../FunctionRoutingApi/ExpectedResponses/Invoking_a_function_bound_to_a_collection_of_derived_entities_or_derived_singleton.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

    /// <summary>
    /// The following request invokes the GetBonus function bound to employee 5 (a manager).
    /// The URL for the function is the function name appended to the derived entity's URL
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-a-derived-entity</remarks>
    [Fact]
    public async Task Invoking_a_function_bound_to_a_derived_entity()
    {
        // Arrange
        const string requestUri = "odata/Employees(5)/API.Examples.OData.FunctionRoutingApi.Models.Manager/GetBonus()";
        string expectedResponse = await File.ReadAllTextAsync("../../../FunctionRoutingApi/ExpectedResponses/Invoking_a_function_bound_to_a_derived_entity.json");
        
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }

}