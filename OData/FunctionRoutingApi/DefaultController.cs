using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.FunctionRoutingApi;

/*
 * In ASP.NET Core, the default route is one where the route prefix is an empty string or null.
 * To associate an unbound action with a configured non-default route, the route template on the controller action should start with the route prefix
 */
/// <summary>
/// 
/// </summary>
/// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/function-routing?tabs=net60%2Cvisual-studio#routing-unbound-edm-functions</remarks>
public class DefaultController : ODataController
{
    /// <summary>
    /// Associates the unbound function GetSalary with the route template odata/GetSalary
    /// </summary>
    /// <param name="hourlyRate"></param>
    /// <param name="hoursWorked"></param>
    /// <returns></returns>
    [HttpGet("odata/GetSalary(hourlyRate={hourlyRate:decimal},hoursWorked={hoursWorked:int})")]
    public ActionResult<decimal> GetSalary(decimal hourlyRate, int hoursWorked)
    {
        return hourlyRate * hoursWorked;
    }
}