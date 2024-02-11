using API.Controllers.EntityFramework;
using Bogus;

namespace API.Controllers;

/// <summary>
/// Generates dummy data for the database
/// </summary>
public class DataGenerator
{
    private static int _customerId = 1;
    public static List<Customer>? Customers { get; set; }
    private static int _orderId = 1;
    public static List<Order>? Orders { get; set; }
    private static int _productId = 1;
    public static List<Product>? Products { get; set; }

    private static Faker? _f;
    
    public static void Generate(int numberOfCustomersToGenerate, int numberOfOrdersPerCustomerToGenerate, int numberOfProductsPerOrderToGenerate)
    {
        _f = new Faker();
        Customers = [];
        Orders = [];
        Products = [];

        for (var i = 0; i < numberOfCustomersToGenerate; i++)
        {
            Customer customer = new()
            {
                Id = _customerId++,
                FirstName = _f.Person.FirstName,
                LastName = _f.Person.LastName,
                Email = _f.Person.Email,
                PhoneNumber = _f.Person.Phone,
                Address = new Address
                {
                    Street = _f.Address.StreetAddress(),
                    City = _f.Address.City(),
                    State = _f.Address.State(),
                    ZipCode = _f.Address.ZipCode()
                }
            };
            Customers.Add(customer);

            for (var j = 0; j < numberOfOrdersPerCustomerToGenerate; j++)
            {
                Order order = new()
                {
                    Id = _orderId++,
                    Name = _f.Commerce.ProductName(),
                    Description = _f.Commerce.ProductAdjective(),
                    Price = _f.Random.Decimal(1, 1000),
                    ShippingAddress = new Address
                    {
                        Street = _f.Address.StreetAddress(),
                        City = _f.Address.City(),
                        State = _f.Address.State(),
                        ZipCode = _f.Address.ZipCode()
                    },
                    Status = _f.PickRandomParam("Pending", "Shipped", "Delivered"),
                    TrackingNumber = _f.Random.AlphaNumeric(10)
                };
                customer.Orders.Add(order);
                Orders.Add(order);

                for (var k = 0; k < numberOfProductsPerOrderToGenerate; k++)
                {
                    Product product = new()
                    {
                        Id = _productId++,
                        Name = _f.Commerce.ProductName(),
                        Description = _f.Commerce.ProductAdjective(),
                        Price = _f.Random.Decimal(1, 1000)
                    };
                    order.Products.Add(product);
                    Products.Add(product);
                }
            }
        }
    }
}