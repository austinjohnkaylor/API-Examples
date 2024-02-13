using API.Controllers.Models.V1;

namespace API.Controllers.EntityFramework;

/// <summary>
/// Represents a product
/// </summary>
public class Product : Audit
{
    /// <summary>
    /// The product's Id
    /// </summary>
    public int Id { get; set; }
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
    
    /// <summary>
    /// Converts a product data transfer object to a <see cref="Product"/>
    /// </summary>
    /// <param name="product">The product data transfer object to convert</param>
    /// <returns>A <see cref="Product"/></returns>
    public static explicit operator Product(ProductDto product)
    {
        return new Product
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category
        };
    }
    
    /// <summary>
    /// Converts a <see cref="Product"/> to a product data transfer object
    /// </summary>
    /// <param name="product">The <see cref="Product"/> to convert</param>
    /// <returns>A product data transfer object</returns>
    public static explicit operator ProductDto(Product product)
    {
        return new ProductDto
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category
        };
    }
}