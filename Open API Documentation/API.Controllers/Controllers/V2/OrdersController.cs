using API.Controllers.EntityFramework;
using API.Controllers.Models.V2;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Controllers.V2
{
    /// <summary>
    /// An API controller for interacting with the Order entity.
    /// </summary>
    /// <param name="context">The SimpleStore database context</param>
    //[Route("api/customers/{customerId:int}/[controller]")]
    [ApiVersion(2.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(SimpleStoreDbContext context) : ControllerBase
    {
        
        /// <summary>
        /// Get all the Orders in the database
        /// </summary>
        /// <returns>A list of Orders</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Order
        ///
        /// </remarks>
        /// <response code="200">Returns all the existing Orders</response>
        /// <response code="400">A bad request was sent</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var Orders = await context.Orders.ToListAsync();
            var OrderDtos = new List<OrderDto>();

            foreach (Order Order in Orders)
            {
                OrderDtos.Add((OrderDto)Order);
            }

            return Ok(OrderDtos);
        }
        
        /// <summary>
        /// Get a specific Order by ID
        /// </summary>
        /// <param name="id">The id of the Order</param>
        /// <returns>a specific Order by ID</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Order/5
        ///
        /// </remarks>
        /// <response code="200">Returns the existing Order</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The Order was not found</response>
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiConventionMethod(typeof(CustomWebApiConventions), nameof(CustomWebApiConventions.Find))] // <- https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#apply-web-api-conventions
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> FindOrder(int id)
        {
            Order? Order = await context.Orders.FindAsync(id);

            if (Order == null)
            {
                return NotFound();
            }
            
            OrderDto OrderDto = (OrderDto)Order;
            return Ok(OrderDto);
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a specific Order by ID
        /// </summary>
        /// <param name="id">The id of the Order</param>
        /// <param name="OrderDto">the Order to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT: api/Order/5
        ///
        /// </remarks>
        /// <response code="204">The Order was updated successfully</response>
        /// <response code="400">The Order's Id doesn't match the rest of the Order's information entered</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The Order was not found</response>
        [Produces("application/json")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))] // <- https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#apply-web-api-conventions
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutOrder(int id, OrderDto OrderDto)
        {
            Order? OrderInDatabase = await context.Orders.FirstOrDefaultAsync(
                order =>
                order.Name == OrderDto.Name && 
                order.Description == OrderDto.Description && 
                order.ShippingAddress.ToString() == OrderDto.Address &&
                order.TrackingNumber == OrderDto.TrackingNumber);

            if (OrderInDatabase == null)
                return NotFound();
            
            if (id != OrderInDatabase.Id)
                return BadRequest();

            context.SetModified(OrderInDatabase);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Orders.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new Order
        /// </summary>
        /// <param name="OrderDto">The Order to create</param>
        /// <returns>The newly created Order</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: api/Order
        ///
        /// </remarks>
        // /// <response code="201">The Order was created successfully</response>
        // /// <response code="400">A bad request was sent</response>
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "The Order was created", typeof(OrderDto))] // <-https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-response-metadata
        [SwaggerResponse(400, "The Order data is invalid")]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> PostOrder([FromBody, SwaggerRequestBody("The Order payload", Required = true)]OrderDto OrderDto) // <- https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-requestbody-metadata
        {
            Order databaseOrder = (Order)OrderDto;
            context.Orders.Add(databaseOrder);
            await context.SaveChangesAsync();

            return CreatedAtAction("FindOrder", new { id = databaseOrder.Id }, databaseOrder);
        }
        
        /// <summary>
        /// Deletes a specific Order by ID
        /// </summary>
        /// <param name="id">The id of the Order to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE: api/Order/5
        ///
        /// </remarks>
        /// <response code="204">The Order was deleted successfully</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The Order was not found</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order? Order = await context.Orders.FindAsync(id);
            if (Order == null)
            {
                return NotFound();
            }

            context.Orders.Remove(Order);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
