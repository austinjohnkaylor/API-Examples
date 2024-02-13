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
    /// Converts an order data transfer object to an <see cref="Order"/>
    /// </summary>
    /// <param name="order">The order data transfer object to convert</param>
    /// <returns>An <see cref="Order"/></returns>
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
    
    /// <summary>
    /// Converts a <see cref="Order"/> to an order data transfer object
    /// </summary>
    /// <param name="order">The <see cref="Order"/> to convert</param>
    /// <returns>An order data transfer object</returns>
    public static explicit operator OrderDto(Order order)
    {
        return new OrderDto
        {
            Name = order.Name,
            Description = order.Description,
            Price = order.Price,
            Address = $"{order.ShippingAddress.Street}, {order.ShippingAddress.City}, {order.ShippingAddress.State}, {order.ShippingAddress.ZipCode}",
            TrackingNumber = order.TrackingNumber
        };
    }
}