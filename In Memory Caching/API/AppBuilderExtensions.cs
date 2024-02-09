using InMemoryCaching.API.Controllers;
using InMemoryCaching.API.EntityFramework;

namespace InMemoryCaching.API;

internal static class AppBuilderExtensions
{
    /// <summary>
    /// Seeds the People database with Data
    /// </summary>
    /// <param name="app"></param>
    /// <param name="numberOfPeopleToGenerate">The number of people to generate in the database</param>
    /// <returns></returns>
    public static void SeedDatabase(this IApplicationBuilder app, int numberOfPeopleToGenerate)
    {
        ArgumentNullException.ThrowIfNull(app, nameof(app));

        using IServiceScope scope = app.ApplicationServices.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            PersonContext context = services.GetRequiredService<PersonContext>();
            DatabaseInitializer.Initialize(context, numberOfPeopleToGenerate);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}