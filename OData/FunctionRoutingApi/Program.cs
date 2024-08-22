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
        
        /*
         * The following code configures an unbound function named GetSalary in the Edm model.
         * The function accepts two parameters, namely, hourlyRate and hoursWorked, and returns a decimal result.
         * Notice that we call Function directly on the ODataModelBuilder, instead of entity type or collection
         *
         * An unbound function can be placed in any controller in the application.
         * To avoid confusion, you can create a controller unassociated with any entity set to serve as a home for your unbound operations.
         */
        FunctionConfiguration? getSalaryFunction = modelBuilder.Function("GetSalary");
        getSalaryFunction.Parameter<decimal>("hourlyRate");
        getSalaryFunction.Parameter<int>("hoursWorked");
        getSalaryFunction.Returns<decimal>();

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