using API.Examples.SharedResources.EntityFramework.ODataBasicCrud;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.SimpleCrudWebApi.Controllers;

/// <summary>
/// An OData controller for interacting with the <see cref="Customer"/>
/// </summary>
public class CustomersController(ODataBasicCrudDbContext db) : ODataController
{
    private readonly ODataBasicCrudDbContext db = db;
    
    public ActionResult<IQueryable<Customer>> Get()
    {
        return Ok(db.Customers);
    }
}