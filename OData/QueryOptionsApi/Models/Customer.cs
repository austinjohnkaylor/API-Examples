using System.ComponentModel.DataAnnotations;
using Bogus;

namespace API.Examples.OData.QueryOptionsApi.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public String Name { get; set; }
    public int Age { get; set; }
    public List<Order> Orders { get; set; }

    /// <summary>
    /// Generates a list of customers with random names, ages, and orders.
    /// </summary>
    /// <param name="numberOfCustomersToGenerate">The number of Customers to generate</param>
    /// <param name="numberOfOrdersPerCustomerToGenerate">The number of Orders to generate per Customer</param>
    /// <returns>A list of randomly-generated Customers</returns>
    public static List<Customer> GenerateCustomers(int numberOfCustomersToGenerate, int numberOfOrdersPerCustomerToGenerate)
    {
        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Age, f => f.Random.Int(18, 65))
            .RuleFor(c => c.Orders, f => Order.GenerateOrders(numberOfOrdersPerCustomerToGenerate));
        
        return customerFaker.Generate(numberOfCustomersToGenerate);
    }
}