using API.Examples.OData.FunctionRoutingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.FunctionRoutingApi;

public class EmployeesController : ODataController
{
    public static List<Employee> employees =
    [
        new Employee { Id = 1, Name = "Employee 1", PerfRating = 8 },
        new Employee { Id = 2, Name = "Employee 2", PerfRating = 7 },
        new Employee { Id = 3, Name = "Employee 3", PerfRating = 5 },
        new Employee { Id = 4, Name = "Employee 4", PerfRating = 3 },
        new Manager { Id = 5, Name = "Employee 5", PerfRating = 7, Bonus = 2900 },
        new Manager { Id = 6, Name = "Employee 6", PerfRating = 9, Bonus = 3700 }
    ];
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-an-entity-set-or-singleton</remarks>
    [HttpGet]
    public ActionResult<decimal> GetHighestRating()
    {
        if (employees.Count < 1)
        {
            return NoContent();
        }

        return employees.Select(d => d.PerfRating).OrderByDescending(d => d).First();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-an-entity</remarks>
    [HttpGet]
    public ActionResult<decimal> GetRating([FromRoute] int key)
    {
        Employee? employee = employees.SingleOrDefault(d => d.Id.Equals(key));

        if (employee == null)
        {
            return NotFound();
        }

        return employee.PerfRating;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-a-collection-of-derived-entities-or-derived-singleton</remarks>
    [HttpGet]
    public ActionResult<decimal> GetHighestBonusOnCollectionOfManager()
    {
        var managers = employees.OfType<Manager>().ToArray();

        if (managers.Length < 1)
        {
            return NoContent();
        }

        return managers.Select(d => d.Bonus).OrderByDescending(d => d).First();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#invoking-a-function-bound-to-a-derived-entity</remarks>
    [HttpGet]
    public ActionResult<decimal> GetBonusOnManager([FromRoute] int key)
    {
        Manager? manager = employees.OfType<Manager>().SingleOrDefault(d => d.Id.Equals(key));

        if (manager == null)
        {
            return NotFound();
        }

        return manager.Bonus;
    }
    
    
}