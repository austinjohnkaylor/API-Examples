using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.ModelBuilder;

namespace API.Examples.SharedResources.OData.CustomSkipTokenHandler;

/// <summary>
/// A static class for adding services to the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Register the custom skip token handler with dependency injection.
    /// </summary>
    /// <param name="services">The dependency injection services container</param>
    /// <param name="modelBuilder">The OData Model Builder</param>
    public static void AddCustomSkipTokenHandler(this IServiceCollection services, ODataConventionModelBuilder modelBuilder)
    {
        // Add the custom skip token handler to the dependency injection container.
        // https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/custom-skiptokenhandler?tabs=net60%2Cvisual-studio#creating-and-registering-the-custom-handler
        services.AddControllers()
            .AddOData(options =>
                options.SkipToken().AddRouteComponents(
                    routePrefix: "odata",
                    model: modelBuilder.GetEdmModel(),
                    configureServices: serviceCollection => serviceCollection.AddSingleton<SkipTokenHandler, CustomSkipTokenHandler>()
                )
            );
        
        
    }
}