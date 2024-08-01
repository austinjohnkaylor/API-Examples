using Bogus;

namespace API.Examples.SharedResources.EntityFramework.ECommerce;

/// <summary>
/// Helper and extension methods for the <see cref="EcommerceDbContext"/> class.
/// </summary>
public static class EcommerceDbContextHelper
{
    /// <summary>
    /// Seeds the Database with a bunch of fake <see cref="Customer"/>, <see cref="Product"/>, <see cref="Order"/>, and <see cref="OrderItem"/> records
    /// </summary>
    /// <param name="db"></param>
    public static void SeedDb(EcommerceDbContext db)
    {
        // Ensure database is created
        db.Database.EnsureCreated();
        
        // Generate fake data
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.ProductId, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => Convert.ToDecimal(f.Commerce.Price()))
            .RuleFor(p => p.Stock, f => f.Random.Int(0, 100));

        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.CustomerId, f => Guid.NewGuid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, f => f.Internet.Email());

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => f.IndexFaker + 1)
            .RuleFor(o => o.OrderDate, f => f.Date.Past())
            .RuleFor(o => o.CustomerId, f => f.PickRandom(db.Customers.Select(c => c.CustomerId).ToList()));

        var orderItemFaker = new Faker<OrderItem>()
            .RuleFor(oi => oi.OrderItemId, f => f.IndexFaker + 1)
            .RuleFor(oi => oi.OrderId, f => f.PickRandom(db.Orders.Select(o => o.OrderId).ToList()))
            .RuleFor(oi => oi.ProductId, f => f.PickRandom(db.Products.Select(p => p.ProductId).ToList()))
            .RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(oi => oi.UnitPrice, (f, oi) => db.Products.First(p => p.ProductId == oi.ProductId).Price);

        // Generate and add data to the context
        var products = productFaker.Generate(50);
        var customers = customerFaker.Generate(20);
        db.Products.AddRange(products);
        db.Customers.AddRange(customers);
        db.SaveChanges();

        var orders = orderFaker.Generate(100);
        db.Orders.AddRange(orders);
        db.SaveChanges();

        var orderItems = orderItemFaker.Generate(200);
        db.OrderItems.AddRange(orderItems);
        db.SaveChanges();
    }
}