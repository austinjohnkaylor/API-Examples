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
    public static CustomerDto CreateInstance()
    {
        return new CustomerDto();
    }
    
    /// <summary>
    /// Converts a customer to a customer DTO
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
}