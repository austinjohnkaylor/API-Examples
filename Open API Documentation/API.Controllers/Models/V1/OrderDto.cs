using API.Controllers.EntityFramework;

namespace API.Controllers.Models.V1;

/// <summary>
/// A data transfer object for an order
/// </summary>
public class OrderDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; }
    public string? TrackingNumber { get; set; }
    
    public static OrderDto CreateInstance()
    {
        return new OrderDto();
    }
    
    public static OrderDto CreateInstance(string name, string description, decimal price, string address, string trackingNumber)
    {
        return new OrderDto
        {
            Name = name,
            Description = description,
            Price = price,
            Address = address,
            TrackingNumber = trackingNumber
        };
    }

    /// <summary>
    /// Converts an Order to an Order DTO
    /// </summary>
    /// <param name="order">The <see cref="Order"/> object in the database</param>
    /// <returns>A <see cref="OrderDto"/></returns>
    public static explicit operator OrderDto(Order order)
    {
        return new OrderDto
        {
            Name = order.Name,
            Description = order.Description,
            Price = order.Price,
            Address = order.ShippingAddress.ToString(),
            TrackingNumber = order.TrackingNumber
        };
    }
}