using Microsoft.AspNetCore.Mvc;
using API.Controllers.EntityFramework;
using API.Controllers.Models.V1;

namespace API.Controllers.Controllers.V1
{
    /// <summary>
    /// An API controller for interacting with the Customer entity.
    /// </summary>
    /// <param name="context"></param>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(SimpleStoreDbContext context) : ControllerBase
    {
        // GET: api/Customer
        /// <summary>
        /// Get all the Customers in the database
        /// </summary>
        /// <returns>A list of <see cref="CustomerDto"/></returns>
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

        // GET: api/Customer/5
        /// <summary>
        /// Get a specific Customer by ID
        /// </summary>
        /// <param name="id">The id of the Customer</param>
        /// <returns>a specific Customer by ID</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            Customer? customer = await context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            
            CustomerDto customerDto = (CustomerDto)customer;
            return Ok(customerDto);
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a specific Customer by ID
        /// </summary>
        /// <param name="id">The id of the Customer</param>
        /// <param name="customerDto">the Customer to update</param>
        /// <returns></returns>
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

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new Customer
        /// </summary>
        /// <param name="customerDto">The Customer to create</param>
        /// <returns>The newly created Customer</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            Customer databaseCustomer = (Customer)customerDto;
            context.Customers.Add(databaseCustomer);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = databaseCustomer.Id }, databaseCustomer);
        }

        // DELETE: api/Customer/5
        /// <summary>
        /// Deletes a specific Customer by ID
        /// </summary>
        /// <param name="id">The id of the Customer to delete</param>
        /// <returns></returns>
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
