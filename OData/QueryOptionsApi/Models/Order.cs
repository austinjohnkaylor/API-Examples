using System.ComponentModel.DataAnnotations;

namespace API.Examples.OData.QueryOptionsApi.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
}