using API.Controllers.EntityFramework;

namespace API.Controllers;

/// <summary>
/// Generates dummy data for the database
/// </summary>
public class DataGenerator
{
    public static void Generate(int numberOfCustomersToGenerate, int numberOfOrdersPerCustomerToGenerate, int numberOfProductsPerOrderToGenerate)
    {
        throw new NotImplementedException();
    }

    public static List<Customer> Customers { get; set; }
    public static List<Order> Orders { get; set; }
    public static List<Product> Products { get; set; }
}