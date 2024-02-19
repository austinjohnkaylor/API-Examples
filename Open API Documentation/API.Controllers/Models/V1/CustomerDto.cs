using API.Controllers.EntityFramework;

namespace API.Controllers.Models.V1;

/// <summary>
/// A data transfer object for a customer
/// </summary>
public class CustomerDto
{
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
    /// Converts a customer to a customer data transfer object
    /// </summary>
    /// <param name="customer">The <see cref="Customer"/> object in the database</param>
    /// <returns>A <see cref="CustomerDto"/></returns>
    public static explicit operator CustomerDto(Customer customer)
    {
        return new CustomerDto
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };
    }

    /// <summary>
    /// Converts a customer data transfer object to a customer
    /// </summary>
    /// <param name="customer">The customer data transfer object</param>
    /// <returns>A <see cref="Customer"/></returns>
    public static explicit operator Customer(CustomerDto customer)
    {
        return new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email
        };
    
    }
}