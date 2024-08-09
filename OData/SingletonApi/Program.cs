using API.Examples.OData.SingletonApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ODataConventionModelBuilder modelBuilder = new();
modelBuilder.Singleton<Company>("Company");

builder.Services.AddControllers()
    .AddOData(options =>
    {
        options.Select().EnableQueryFeatures(null).AddRouteComponents(
            routePrefix: "odata",
            model: modelBuilder.GetEdmModel());
    });

WebApplication app = builder.Build();

app.UseODataRouteDebug();
app.UseRouting();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints => endpoints.MapControllers());
#pragma warning restore ASP0014

app.Run();

// Needed for Integration Testing
namespace API.Examples.OData.SingletonApi
{
    public partial class Program;
}