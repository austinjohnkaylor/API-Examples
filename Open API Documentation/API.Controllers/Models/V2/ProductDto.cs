using API.Controllers.EntityFramework;

namespace API.Controllers.Models.V2;

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
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
        };
    }
}