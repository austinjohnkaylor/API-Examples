using InMemoryCaching.API.EntityFramework;

namespace InMemoryCaching.API;

public class DatabaseInitializer
{
    /// <summary>
    /// Initializes the People database and makes sure it has data in it
    /// </summary>
    /// <param name="dbContext">The <see cref="PersonContext"/> Entity Framework DB Context</param>
    /// <param name="numberOfPeopleToGenerate">The number of people to generate in the database</param>
    internal static void Initialize(PersonContext dbContext, int numberOfPeopleToGenerate)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
       
        dbContext.Database.EnsureCreated();
            
        if (dbContext.People.Any())
            return;
            
        PersonGenerator.Generate(numberOfPeopleToGenerate);

        dbContext.People.AddRange(PersonGenerator.People);

        dbContext.SaveChanges();
    }
}