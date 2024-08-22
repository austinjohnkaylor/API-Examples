namespace API.Examples.SharedResources.EntityFramework.ECommerce;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ICollection<Order> Orders { get; set; } // Navigation property
}