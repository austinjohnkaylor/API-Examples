using API.Controllers.EntityFramework;

namespace API.Controllers.Models.V1;

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public static ProductDto CreateInstance()
    {
        return new ProductDto();
    }
    
    public static ProductDto CreateInstance(string name, string description, decimal price, string category)
    {
        return new ProductDto
        {
            Name = name,
            Description = description,
            Price = price,
            Category = category
        };
    }

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