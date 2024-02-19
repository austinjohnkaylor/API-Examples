using API.Controllers.EntityFramework;

namespace API.Controllers.Models.V3;

/// <summary>
/// A data transfer object for an order
/// </summary>
public class OrderDto
{
    public OrderDto()
    {
        OrderTag = $"{Name} - {TrackingNumber}";
    }

    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; }
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Is a combination of the order's name and tracking number
    /// </summary>
    public string OrderTag { get; set; }

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
            Price = order.Price,
            Address = $"{order.ShippingAddress.Street}, {order.ShippingAddress.City}, {order.ShippingAddress.State}, {order.ShippingAddress.ZipCode}",
            TrackingNumber = order.TrackingNumber
        };
    }
}