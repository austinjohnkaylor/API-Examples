using API.Examples.OData.NavigationRoutingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
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
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#retrieving-a-single-valued-or-collection-valued-navigation-property-on-an-entity</remarks>
    public ActionResult<Employee> GetSupervisor([FromRoute] int key)
    {
        Employee? employee = employees.SingleOrDefault(d => d.Id.Equals(key));

        return employee?.Supervisor ?? (ActionResult<Employee>)NotFound();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#retrieving-a-single-valued-or-collection-valued-navigation-property-on-a-derived-entity</remarks>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#retrieving-the-count-of-a-collection-valued-navigation-property-on-a-derived-entity</remarks>
    [EnableQuery]
    public ActionResult<IEnumerable<Employee>> GetDirectReportsFromManager([FromRoute] int key)
    {
        Manager? manager = employees.OfType<Manager>().SingleOrDefault(d => d.Id.Equals(key));

        if (manager == null)
        {
            return NotFound();
        }

        return manager.DirectReports;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="peer"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#post-to-a-collection-valued-navigation-property-on-an-entity</remarks>
    public ActionResult PostToPeers([FromRoute] int key, [FromBody] Employee peer)
    {
        Employee? employee = employees.SingleOrDefault(d => d.Id.Equals(key));

        if (employee == null)
        {
            return NotFound();
        }

        employees.Add(peer);
        employee.Peers.Add(peer);

        return Created(peer);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="employee"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#post-to-a-collection-valued-navigation-property-on-a-derived-entity</remarks>
    public ActionResult PostToDirectReportsFromManager([FromRoute] int key, [FromBody] Employee employee)
    {
        Manager? manager = employees.OfType<Manager>().SingleOrDefault(d => d.Id.Equals(key));

        if (manager == null)
        {
            return NotFound();
        }

        employees.Add(employee);
        manager.DirectReports.Add(employee);

        return Created(employee);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="supervisor"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#updating-a-single-valued-navigation-property-on-an-entity</remarks>
    public ActionResult PutToSupervisor([FromRoute] int key, [FromBody] Employee supervisor)
    {
        Employee? employee = employees.SingleOrDefault(d => d.Id.Equals(key));

        if (employee == null)
        {
            return NotFound();
        }

        employee.Supervisor = supervisor;

        return Ok();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="personalAssistant"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#updating-a-single-valued-navigation-property-on-a-derived-entity</remarks>
    public ActionResult PutToPersonalAssistantFromManager([FromRoute] int key, [FromBody] Employee personalAssistant)
    {
        Manager? manager = employees.OfType<Manager>().SingleOrDefault(d => d.Id.Equals(key));

        if (manager == null)
        {
            return NotFound();
        }

        manager.PersonalAssistant = personalAssistant;

        return Ok();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#patching-a-navigation-property-on-an-entity</remarks>
    public ActionResult PatchToSupervisor([FromRoute] int key, [FromBody] Delta<Employee> delta)
    {
        Employee? employee = employees.SingleOrDefault(d => d.Id.Equals(key));

        if (employee == null)
        {
            return NotFound();
        }

        delta.Patch(employee.Supervisor);

        return Ok();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/navigation-routing?tabs=net60%2Cvisual-studio#patching-a-navigation-property-on-a-derived-entity</remarks>
    public ActionResult PatchToPersonalAssistantFromManager([FromRoute] int key, [FromBody] Delta<Employee> delta)
    {
        Manager? manager = employees.OfType<Manager>().SingleOrDefault(d => d.Id.Equals(key));

        if (manager == null)
        {
            return NotFound();
        }

        if (manager.PersonalAssistant != null)
        {
            delta.Patch(manager.PersonalAssistant);
        }

        return Ok();
    }
}