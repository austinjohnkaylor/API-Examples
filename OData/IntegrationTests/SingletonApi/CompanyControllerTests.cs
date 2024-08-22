using System.Net.Http.Headers;
using API.Examples.OData.IntegrationTests.Extensions;
using API.Examples.OData.SingletonApi.Controllers;
using API.Examples.OData.SingletonApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Prog = API.Examples.OData.SingletonApi.Program;

namespace API.Examples.OData.IntegrationTests.SingletonApi;

/// <summary>
/// Integration Tests for the <see cref="CompanyController"/>
/// </summary>
public class CompanyControllerTests : IClassFixture<WebApplicationFactory<Prog>>, IDisposable
{
    private readonly HttpClient _httpClient;

    public CompanyControllerTests(WebApplicationFactory<Prog> application)
    {
        _httpClient = application.CreateClient();
        CompanyController.Company = new HoldingCompany
        {
            Id = 13,
            Name = "Company LLC",
            NumberOfSubsidiaries = 7
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#retrieving-a-singleton</remarks>
    [Fact]
    public async Task Retrieving_a_Singleton()
    {
        const string requestUri = "odata/Company";
        string expectedResponse = await File.ReadAllTextAsync("../../../SingletonApi/ExpectedResponses/Retrieving a Singleton.json");
        
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
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#retrieving-a-derived-singleton</remarks>
    [Fact]
    public async Task Retrieving_a_Derived_Singleton()
    {
        // Arrange
        const string requestUri = "odata/Company/API.Examples.OData.SingletonApi.Models.HoldingCompany";
        string expectedResponse = await File.ReadAllTextAsync("../../../SingletonApi/ExpectedResponses/Retrieving a derived Singleton.json");
        // Act
        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

        // Asset
        response.EnsureSuccessStatusCode();
        string actualResponse = await response.Content.ReadAsStringAsync();
        Assert.Equal(expectedResponse.ToFormattedJsonString(), actualResponse.ToFormattedJsonString());
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#updating-a-singleton</remarks>
    [Fact]
    public async Task Updating_a_Singleton()
    {
        // Arrange
        const string requestUri = "odata/Company";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../SingletonApi/RequestBodies/Updating a Singleton.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PutAsync(requestUri, requestBody);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#updating-a-derived-singleton</remarks>
    [Fact]
    public async Task Updating_a_Derived_Singleton()
    {
        // Arrange
        const string requestUri = "odata/Company/API.Examples.OData.SingletonApi.Models.HoldingCompany";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../SingletonApi/RequestBodies/Updating a derived Singleton.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PutAsync(requestUri, requestBody);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#patching-a-singleton</remarks>
    [Fact]
    public async Task Patching_a_Singleton()
    {
        // Arrange
        const string requestUri = "odata/Company";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../SingletonApi/RequestBodies/Patching a Singleton.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#patching-a-derived-singleton</remarks>
    [Fact]
    public async Task Patching_a_Derived_Singleton()
    {
        // Arrange
        const string requestUri = "odata/Company/API.Examples.OData.SingletonApi.Models.HoldingCompany";
        HttpContent requestBody =
            new StringContent(
                await File.ReadAllTextAsync("../../../SingletonApi/RequestBodies/Patching a derived Singleton.json"));
        requestBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        // Act
        HttpResponseMessage response = await _httpClient.PatchAsync(requestUri, requestBody);
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
    
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _httpClient.Dispose();
    }
}