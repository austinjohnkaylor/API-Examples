using Microsoft.AspNetCore.Mvc;
using API.Controllers.EntityFramework;
using API.Controllers.Models.V1;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Controllers.V1
{
    /// <summary>
    /// An API controller for interacting with the Customer entity.
    /// </summary>
    /// <param name="context">The SimpleStore database context</param>
    [ApiVersion( 1.0 )]
    [ApiVersion( 0.9, Deprecated = true )]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(SimpleStoreDbContext context) : ControllerBase
    {
        
        /// <summary>
        /// Get all the Customers in the database
        /// </summary>
        /// <returns>A list of Customers</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Customer
        ///
        /// </remarks>
        /// <response code="200">Returns all the existing customers</response>
        /// <response code="400">A bad request was sent</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            var customers = await context.Customers.ToListAsync();
            var customerDtos = new List<CustomerDto>();

            foreach (Customer customer in customers)
            {
                customerDtos.Add((CustomerDto)customer);
            }

            return Ok(customerDtos);
        }
        
        /// <summary>
        /// Get a specific Customer by ID
        /// </summary>
        /// <param name="id">The id of the Customer</param>
        /// <returns>a specific Customer by ID</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Customer/5
        ///
        /// </remarks>
        /// <response code="200">Returns the existing customer</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The customer was not found</response>
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiConventionMethod(typeof(CustomWebApiConventions), nameof(CustomWebApiConventions.Find))] // <- https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#apply-web-api-conventions
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDto>> FindCustomer(int id)
        {
            Customer? customer = await context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            
            CustomerDto customerDto = (CustomerDto)customer;
            return Ok(customerDto);
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a specific Customer by ID
        /// </summary>
        /// <param name="id">The id of the Customer</param>
        /// <param name="customerDto">the Customer to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT: api/Customer/5
        ///
        /// </remarks>
        /// <response code="204">The customer was updated successfully</response>
        /// <response code="400">The customer's Id doesn't match the rest of the customer's information entered</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The customer was not found</response>
        [Produces("application/json")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))] // <- https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#apply-web-api-conventions
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerDto customerDto)
        {
            Customer? customerInDatabase = await context.Customers.FirstOrDefaultAsync(
                c =>
                c.FirstName == customerDto.FirstName && 
                c.LastName == customerDto.LastName && 
                c.Email == customerDto.Email);

            if (customerInDatabase == null)
                return NotFound();
            
            if (id != customerInDatabase.Id)
                return BadRequest();

            context.SetModified(customerInDatabase);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Customers.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new Customer
        /// </summary>
        /// <param name="customerDto">The Customer to create</param>
        /// <returns>The newly created Customer</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: api/Customer
        ///
        /// </remarks>
        // /// <response code="201">The customer was created successfully</response>
        // /// <response code="400">A bad request was sent</response>
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "The customer was created", typeof(CustomerDto))] // <-https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-response-metadata
        [SwaggerResponse(400, "The customer data is invalid")]
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer([FromBody, SwaggerRequestBody("The customer payload", Required = true)]CustomerDto customerDto) // <- https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-requestbody-metadata
        {
            Customer databaseCustomer = (Customer)customerDto;
            context.Customers.Add(databaseCustomer);
            await context.SaveChangesAsync();

            return CreatedAtAction("FindCustomer", new { id = databaseCustomer.Id }, databaseCustomer);
        }
        
        /// <summary>
        /// Deletes a specific Customer by ID
        /// </summary>
        /// <param name="id">The id of the Customer to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE: api/Customer/5
        ///
        /// </remarks>
        /// <response code="204">The customer was deleted successfully</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The customer was not found</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            Customer? customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
