namespace API.Examples.OData.NavigationRoutingApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Employee Supervisor { get; set; }
    public List<Employee> Peers { get; set; } = new List<Employee>();
}