namespace API.Examples.SharedResources.EntityFramework;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } // Navigation property
}