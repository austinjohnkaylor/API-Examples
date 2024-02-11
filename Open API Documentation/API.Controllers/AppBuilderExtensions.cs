using API.Controllers.EntityFramework;

namespace API.Controllers;

internal static class AppBuilderExtensions
{
    /// <summary>
    /// Seeds the People database with Data
    /// </summary>
    /// <param name="app"></param>
    /// <param name="numberOfCustomersToGenerate"></param>
    /// <param name="numberOfOrdersPerCustomerToGenerate"></param>
    /// <param name="numberOfProductsPerOrderToGenerate"></param>
    /// <returns></returns>
    public static void SeedDatabase(this IApplicationBuilder app, int numberOfCustomersToGenerate, int numberOfOrdersPerCustomerToGenerate, int numberOfProductsPerOrderToGenerate)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using IServiceScope scope = app.ApplicationServices.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            SimpleStoreDbContext context = services.GetRequiredService<SimpleStoreDbContext>();
            DatabaseInitializer.Initialize(context, numberOfCustomersToGenerate, numberOfOrdersPerCustomerToGenerate,
                numberOfProductsPerOrderToGenerate);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}