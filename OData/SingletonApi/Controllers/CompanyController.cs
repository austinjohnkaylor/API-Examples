using API.Examples.OData.SingletonApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.SingletonApi.Controllers;

public class CompanyController : ODataController
{
    public static Company Company;

    /// <summary>
    /// static constructor that is called only once in the lifetime of the service
    /// </summary>
    static CompanyController()
    {
        // We are initializing the company static class member to an instance of the derived type HoldingCompany
        Company = new HoldingCompany
        {
            Id = 13,
            Name = "Company LLC",
            NumberOfSubsidiaries = 7
        };
    }
    
    /// <summary>
    /// Retrieving a singleton
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#retrieving-a-singleton</remarks>
    /// <returns>the <see cref="Company"/> singleton</returns>
    /// <example>
    /// GET http://localhost:5000/odata/Company
    /// </example>
    /// <value> GET ~/{singleton}</value>
    public ActionResult<Company> Get()
    {
        return Company;
    }
    
    /// <summary>
    /// Retrieving a derived singleton
    /// </summary>
    /// <returns> the <see cref="HoldingCompany"/> derived singleton</returns>
    /// <value>GET ~/{singleton}/{cast}</value>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#retrieving-a-derived-singleton</remarks>
    /// <example>
    /// GET http://localhost:5000/odata/Company/SingletonApi.Models.HoldingCompany
    /// </example>
    public ActionResult<HoldingCompany> GetFromHoldingCompany()
    {
        if (Company is not HoldingCompany holdingCompany)
        {
            return NotFound();
        }

        return holdingCompany;
    }
    
    /// <summary>
    /// Updating a singleton
    /// </summary>
    /// <param name="updated">The updated <see cref="Models.Company"/> singleton</param>
    /// <returns>updates the Company singleton</returns>
    /// <value>PUT ~/{singleton}</value>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#updating-a-singleton</remarks>
    /// <example>PUT http://localhost:5000/odata/Company</example>
    public ActionResult Put([FromBody] Company updated)
    {
        Company.Name = updated.Name;

        return Ok();
    }
    
    /// <summary>
    /// Updating a derived singleton
    /// </summary>
    /// <param name="updated">The updated derived singleton <see cref="HoldingCompany"/></param>
    /// <returns>Updates the <see cref="HoldingCompany"/> derived singleton</returns>
    /// <value>PUT ~/{singleton}/{cast}</value>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#updating-a-derived-singleton</remarks>
    /// <example>PUT http://localhost:5000/odata/Company/SingletonApi.Models.HoldingCompany</example>
    public ActionResult PutFromHoldingCompany([FromBody] HoldingCompany updated)
    {
        if (Company is not HoldingCompany holdingCompany)
        {
            return NotFound();
        }

        holdingCompany.Name = updated.Name;
        holdingCompany.NumberOfSubsidiaries = updated.NumberOfSubsidiaries;

        return Ok();
    }
    
    /// <summary>
    /// Patching a singleton
    /// </summary>
    /// <param name="delta">The <see cref="Delta{T}"/> of the singleton <see cref="Company"/></param>
    /// <returns></returns>
    /// <value>PATCH ~/{singleton}</value>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#patching-a-singleton</remarks>
    /// <example>PATCH http://localhost:5000/odata/Company</example>
    public ActionResult Patch([FromBody] Delta<Company> delta)
    {
        delta.Patch(Company);

        return Ok();
    }
    
    /// <summary>
    /// Patching a derived singleton
    /// </summary>
    /// <param name="delta">The <see cref="Delta{T}"/> of the derived singleton <see cref="HoldingCompany"/></param>
    /// <returns></returns>
    /// <value>PATCH ~/{singleton}/{cast}</value>
    /// <example>PATCH http://localhost:5000/odata/Company/SingletonRouting.Models.HoldingCompany</example>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/singleton-routing?tabs=visual-studio%2Cnet60#patching-a-derived-singleton</remarks>
    public ActionResult PatchFromHoldingCompany([FromBody] Delta<HoldingCompany> delta)
    {
        if (Company is not HoldingCompany holdingCompany)
        {
            return NotFound();
        }

        delta.Patch(holdingCompany);

        return Ok();
    }
    
    
}