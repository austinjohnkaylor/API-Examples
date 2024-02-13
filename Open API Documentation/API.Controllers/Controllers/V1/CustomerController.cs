using Microsoft.AspNetCore.Mvc;
using API.Controllers.EntityFramework;
using API.Controllers.Models.V1;

namespace API.Controllers.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(SimpleStoreDbContext context) : ControllerBase
    {
        // GET: api/Customer
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
        [HttpGet("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> PostCustomer(CustomerDto customerDto)
        {
            Customer databaseCustomer = (Customer)customerDto;
            context.Customers.Add(databaseCustomer);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = databaseCustomer.Id }, databaseCustomer);
        }

        // DELETE: api/Customer/5
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
