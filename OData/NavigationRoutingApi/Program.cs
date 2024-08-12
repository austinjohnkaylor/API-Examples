using API.Examples.OData.NavigationRoutingApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace API.Examples.OData.NavigationRoutingApi;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        ODataConventionModelBuilder modelBuilder = new();
        modelBuilder.EntitySet<Employee>("Employees");

        builder.Services.AddControllers().AddOData(
            options => options.EnableQueryFeatures(null).AddRouteComponents(
                routePrefix: "odata",
                model: modelBuilder.GetEdmModel()));
        
        WebApplication app = builder.Build();
        
        app.UseODataRouteDebug();
        app.UseRouting();
#pragma warning disable ASP0014
        app.UseEndpoints(endpoints => endpoints.MapControllers());
#pragma warning restore ASP0014

        app.Run();
    }
}