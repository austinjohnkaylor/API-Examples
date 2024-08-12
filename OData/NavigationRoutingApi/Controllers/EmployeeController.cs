using API.Examples.OData.NavigationRoutingApi.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.NavigationRoutingApi.Controllers;

/// <summary>
/// 
/// </summary>
/// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#controller</remarks>
public class EmployeesController : ODataController
{
    private static IList<Employee> employees = GetEmployees();

    private static List<Employee> GetEmployees()
    {
        Manager employee5 = new() { Id = 5, Name = "Employee 5" };
        Employee employee1 = new() { Id = 1, Name = "Employee 1", Supervisor = employee5 };
        Employee employee2 = new() { Id = 2, Name = "Employee 2", Supervisor = employee5 };
        Employee employee3 = new() { Id = 3, Name = "Employee 3", Supervisor = employee5 };
        Employee employee4 = new() { Id = 4, Name = "Employee 4" }; // No Supervisor
        Manager employee6 = new() { Id = 6, Name = "Employee 6" };

        employee5.DirectReports = [employee1, employee2, employee3];
        employee5.PersonalAssistant = employee3;

        return [employee1, employee2, employee3, employee4, employee5, employee6];
    }
}