using System.ComponentModel.DataAnnotations;

namespace API.Examples.OData.QueryOptionsApi.Models;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public String Name { get; set; }
    public int Age { get; set; }
    public List<Order> Orders { get; set; }
}