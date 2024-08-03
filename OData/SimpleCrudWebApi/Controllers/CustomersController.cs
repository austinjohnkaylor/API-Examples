using API.Examples.SharedResources.EntityFramework.ODataBasicCrud;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.SimpleCrudWebApi.Controllers;

/// <summary>
/// An OData controller for interacting with the <see cref="Customer"/>
/// </summary>
public class CustomersController(ODataBasicCrudDbContext db) : ODataController
{
    private readonly ODataBasicCrudDbContext db = db;
    
    /// <summary>
    /// Get all <see cref="Customer"/>s
    /// </summary>
    /// <returns></returns>
    [EnableQuery]
    public ActionResult<IQueryable<Customer>> Get()
    {
        return Ok(db.Customers);
    }

    /// <summary>
    /// Get an individual <see cref="Customer"/> by ID
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#request-a-single-entity</remarks>
    public ActionResult GetById([FromRoute] int key)
    {
        Customer? customer = db.Customers.Find(key);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
    
    /// <summary>
    /// Create a new <see cref="Customer"/>
    /// </summary>
    /// <param name="customer"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#create-an-entity</remarks>
    public ActionResult Post([FromBody] Customer customer)
    {
        db.Customers.Add(customer);
    
        db.SaveChanges();

        return Created(customer);
    }
    
    /// <summary>
    /// Update a <see cref="Customer"/> by ID
    /// </summary>
    /// <param name="key"></param>
    /// <param name="updatedCustomer"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#update-an-entity-using-put</remarks>
    public ActionResult Put([FromRoute] int key, [FromBody] Customer updatedCustomer)
    {
        Customer? customer = db.Customers.SingleOrDefault(d => d.Id == key);

        if (customer == null)
        {
            return NotFound();
        }

        customer.Name = updatedCustomer.Name;
        customer.CustomerType = updatedCustomer.CustomerType;
        customer.CreditLimit = updatedCustomer.CreditLimit;
        customer.CustomerSince = updatedCustomer.CustomerSince;

        db.SaveChanges();

        return Updated(customer);
    }
    
    /// <summary>
    /// Update a <see cref="Customer"/> by ID and from the Delta
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#update-an-entity-using-patch</remarks>
    public ActionResult Patch([FromRoute] int key, [FromBody] Delta<Customer> delta)
    {
        Customer? customer = db.Customers.SingleOrDefault(d => d.Id == key);

        if (customer == null)
        {
            return NotFound();
        }

        delta.Patch(customer);

        db.SaveChanges();

        return Updated(customer);
    }
    
    /// <summary>
    /// Delete a <see cref="Customer"/> by ID
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#delete-an-entity</remarks>
    public ActionResult Delete([FromRoute] int key)
    {
        Customer? customer = db.Customers.SingleOrDefault(d => d.Id == key);

        if (customer != null)
        {
            db.Customers.Remove(customer);
        }

        db.SaveChanges();

        return NoContent();
    }
}