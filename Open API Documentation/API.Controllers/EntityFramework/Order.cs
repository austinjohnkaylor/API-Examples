using API.Controllers.Models.V1;

namespace API.Controllers.EntityFramework;

/// <summary>
/// Represents an order
/// </summary>
public class Order : Audit
{
    /// <summary>
    /// The order's Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The order's name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// A brief description of the order
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// The total price of the order
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// The address the order is being shipped to
    /// </summary>
    public Address ShippingAddress { get; set; }
    /// <summary>
    /// The status of the order
    /// </summary>
    public string? Status { get; set; }
    /// <summary>
    /// A shipping tracking number for the order
    /// </summary>
    public string? TrackingNumber { get; set; }
    /// <summary>
    /// Represents products in an order
    /// </summary>
    public ICollection<Product> Products { get; set; }
    
    public static explicit operator Order(OrderDto order)
    {
        return new Order
        {
            Name = order.Name,
            Description = order.Description,
            Price = order.Price,
            ShippingAddress = new Address
            {
                Street = order.Address.Split(",")[0],
                City = order.Address.Split(",")[1],
                State = order.Address.Split(",")[2],
                ZipCode = order.Address.Split(",")[3]
            },
            TrackingNumber = order.TrackingNumber
        };
    }
}