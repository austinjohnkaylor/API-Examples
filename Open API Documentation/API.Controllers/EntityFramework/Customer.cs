namespace API.Controllers.EntityFramework;

/// <summary>
/// Represents a customer
/// </summary>
public class Customer : Audit
{
    /// <summary>
    /// The customer's Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The customer's first name
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// The customer's last name
    /// </summary>
    public string LastName { get; set; }
    /// <summary>
    /// The customer's e-mail address
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// The customer's phone number
    /// </summary>
    public string PhoneNumber { get; set; }
    /// <summary>
    /// The customer's address
    /// </summary>
    public string Address { get; set; }
    /// <summary>
    /// The customer's city
    /// </summary>
    public string City { get; set; }
    /// <summary>
    /// The customer's state
    /// </summary>
    public string State { get; set; }
    /// <summary>
    /// The customer's zip code
    /// </summary>
    public string ZipCode { get; set; }
    /// <summary>
    /// Represents a customer's orders
    /// </summary>
    public ICollection<Order> Orders { get; set; }
}