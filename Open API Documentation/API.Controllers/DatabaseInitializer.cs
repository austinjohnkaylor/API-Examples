using API.Controllers.EntityFramework;

namespace API.Controllers;

public class DatabaseInitializer
{
    /// <summary>
    /// Initializes the People database and makes sure it has data in it
    /// </summary>
    /// <param name="dbContext">The <see cref="EntityFramework.SimpleStoreDbContext"/> Entity Framework DB Context</param>
    /// <param name="numberOfCustomersToGenerate"></param>
    /// <param name="numberOfOrdersPerCustomerToGenerate"></param>
    /// <param name="numberOfProductsPerOrderToGenerate"></param>
    internal static void Initialize(SimpleStoreDbContext dbContext, int numberOfCustomersToGenerate, int numberOfOrdersPerCustomerToGenerate, int numberOfProductsPerOrderToGenerate)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
       
        dbContext.Database.EnsureCreated();
            
        if (dbContext.Customers.Any())
            return;
            
        DataGenerator.Generate(numberOfCustomersToGenerate, numberOfOrdersPerCustomerToGenerate, numberOfProductsPerOrderToGenerate);

        dbContext.Customers.AddRange(DataGenerator.Customers);

        dbContext.SaveChanges();
    }
}