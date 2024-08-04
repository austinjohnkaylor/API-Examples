using API.Examples.SharedResources.EntityFramework.ODataBasicCrud;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace API.Examples.OData.SimpleCrudWebApi.Controllers;

/// <summary>
/// An OData controller for interacting with the <see cref="Customer"/>
/// </summary>
public class CustomersController(ODataBasicCrudDbContext db) : ODataController
{
    /// <summary>
    /// Get all <see cref="Customer"/>s
    /// </summary>
    /// <returns></returns>
    [EnableQuery]
    public async Task<ActionResult<IQueryable<Customer>>> Get()
    {
        return Ok(await db.Customers.AsQueryable().ToListAsync());
    }

    /// <summary>
    /// Get an individual <see cref="Customer"/> by ID
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#request-a-single-entity</remarks>
    public async Task<ActionResult> GetById([FromRoute] int key)
    {
        Customer? customer = await db.Customers.FindAsync(key);

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
    public async Task<ActionResult> Post([FromBody] Customer customer)
    {
        await db.Customers.AddAsync(customer);
    
        await db.SaveChangesAsync();

        return Created(customer);
    }
    
    /// <summary>
    /// Update a <see cref="Customer"/> by ID
    /// </summary>
    /// <param name="key"></param>
    /// <param name="updatedCustomer"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#update-an-entity-using-put</remarks>
    public async Task<ActionResult> Put([FromRoute] int key, [FromBody] Customer updatedCustomer)
    {
        Customer? customer = await db.Customers.FindAsync(key);

        if (customer == null)
        {
            return NotFound();
        }

        customer.Name = updatedCustomer.Name;
        customer.CustomerType = updatedCustomer.CustomerType;
        customer.CreditLimit = updatedCustomer.CreditLimit;
        customer.CustomerSince = updatedCustomer.CustomerSince;

        await db.SaveChangesAsync();

        return Updated(customer);
    }
    
    /// <summary>
    /// Update a <see cref="Customer"/> by ID and from the Delta
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#update-an-entity-using-patch</remarks>
    public async Task<ActionResult> Patch([FromRoute] int key, [FromBody] Delta<Customer> delta)
    {
        Customer? customer = await db.Customers.FindAsync(key);

        if (customer == null)
        {
            return NotFound();
        }

        delta.Patch(customer);

        await db.SaveChangesAsync();

        return Updated(customer);
    }
    
    /// <summary>
    /// Delete a <see cref="Customer"/> by ID
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#delete-an-entity</remarks>
    public async Task<ActionResult> Delete([FromRoute] int key)
    {
        Customer? customer = await db.Customers.FindAsync(key);

        if (customer != null)
        {
            db.Customers.Remove(customer);
        }

        await db.SaveChangesAsync();

        return NoContent();
    }
}