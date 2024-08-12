using Microsoft.AspNetCore.Mvc.Testing;
using Prog = API.Examples.OData.NavigationRoutingApi.Program;

namespace API.Examples.OData.IntegrationTests.NavigationRoutingApi;

/// <summary>
/// Integration tests for the <see cref="API.Examples.OData.NavigationRoutingApi.Controllers.EmployeesController"/>
/// </summary>
public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Prog>>, IDisposable
{
    private readonly HttpClient _httpClient = new WebApplicationFactory<Prog>().CreateClient();

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}