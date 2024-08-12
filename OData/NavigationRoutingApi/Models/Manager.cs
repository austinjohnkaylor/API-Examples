namespace API.Examples.OData.NavigationRoutingApi.Models;

public class Manager : Employee
{
    public Employee PersonalAssistant { get; set; }
    public List<Employee> DirectReports { get; set; } = [];
}