namespace API.Controllers.EntityFramework;

/// <summary>
/// Represents a product
/// </summary>
public class Product : Audit
{
    /// <summary>
    /// The product's Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The product's name
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// A short description about the product
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// The price of the product
    /// </summary>
    public decimal Price { get; set; }
    /// <summary>
    /// The cost to make the product
    /// </summary>
    public decimal Cost { get; set; }
    /// <summary>
    /// An image associated with the product
    /// </summary>
    public string Image { get; set; }
    /// <summary>
    /// The category the product falls into
    /// </summary>
    public string Category { get; set; }
}