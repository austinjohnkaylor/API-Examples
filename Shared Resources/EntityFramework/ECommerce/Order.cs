namespace API.Examples.SharedResources.EntityFramework.ECommerce;

public class Order
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid CustomerId { get; set; } // Foreign key
    public Customer Customer { get; set; } // Navigation property
    public ICollection<OrderItem> OrderItems { get; set; } // Navigation property
}