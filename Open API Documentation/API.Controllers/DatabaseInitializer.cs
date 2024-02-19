using API.Controllers.EntityFramework;

namespace API.Controllers;

public class DatabaseInitializer
{
    /// <summary>
    /// Initializes the Simple Store database and makes sure it has data in it
    /// </summary>
    /// <param name="dbContext">The <see cref="EntityFramework.SimpleStoreDbContext"/> Entity Framework DB Context</param>
    /// <param name="numberOfCustomersToGenerate">The number of <see cref="Customer"/> records to generate</param>
    /// <param name="numberOfOrdersPerCustomerToGenerate">The number of <see cref="Order"/> records to generate per <see cref="Customer"/></param>
    /// <param name="numberOfProductsPerOrderToGenerate">The number of <see cref="Product"/> reocrds to generate per <see cref="Order"/></param>
    internal static void Initialize(SimpleStoreDbContext dbContext, int numberOfCustomersToGenerate, int numberOfOrdersPerCustomerToGenerate, int numberOfProductsPerOrderToGenerate)
    {
        ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
       
        dbContext.Database.EnsureCreated();
            
        if (dbContext.Customers.Any())
            return;
            
        DataGenerator.Generate(numberOfCustomersToGenerate, numberOfOrdersPerCustomerToGenerate, numberOfProductsPerOrderToGenerate);

        dbContext.Customers.AddRange(DataGenerator.Customers ?? []);
        dbContext.Orders.AddRange(DataGenerator.Orders ?? []);
        dbContext.Products.AddRange(DataGenerator.Products ?? []);

        dbContext.SaveChanges();
    }
}