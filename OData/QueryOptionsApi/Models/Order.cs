using System.ComponentModel.DataAnnotations;
using Bogus;

namespace API.Examples.OData.QueryOptionsApi.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Generates a list of orders with random prices and quantities.
    /// </summary>
    /// <param name="numberOfOrdersToGenerate">The number of Orders to Generate</param>
    /// <returns>A list of randomly Generated Orders</returns>
    public static List<Order> GenerateOrders(int numberOfOrdersToGenerate)
    {
        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.Id, f => f.IndexFaker + 1)
            .RuleFor(o => o.Price, f => f.Random.Int(1, 100))
            .RuleFor(o => o.Quantity, f => f.Random.Int(1, 10));
        
        return orderFaker.Generate(numberOfOrdersToGenerate);
    }
}