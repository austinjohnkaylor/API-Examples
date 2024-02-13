using API.Controllers.EntityFramework;

namespace API.Controllers.Models.V1;

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}