using API.Examples.OData.FunctionRoutingApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace API.Examples.OData.FunctionRoutingApi;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ODataConventionModelBuilder modelBuilder = new();
        var employeeEntityType = modelBuilder.EntitySet<Employee>("Employees").EntityType;
        var managerEntityType = modelBuilder.EntityType<Manager>();

        employeeEntityType.Collection.Function("GetHighestRating")
            .Returns<int>();
        employeeEntityType.Function("GetRating")
            .Returns<int>();
        managerEntityType.Collection.Function("GetHighestBonus")
            .Returns<decimal>();
        managerEntityType.Function("GetBonus")
            .Returns<decimal>();

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