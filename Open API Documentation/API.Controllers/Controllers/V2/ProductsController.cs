using API.Controllers.EntityFramework;
using API.Controllers.Models.V2;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Controllers.V2
{
    /// <summary>
    /// An API controller for interacting with the Product entity.
    /// </summary>
    /// <param name="context">The SimpleStore database context</param>
    //[Route("api/customers/{customerId:int}/[controller]")]
    [ApiVersion(2.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(SimpleStoreDbContext context) : ControllerBase
    {
        
        /// <summary>
        /// Get all the Products in the database
        /// </summary>
        /// <returns>A list of Products</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Product
        ///
        /// </remarks>
        /// <response code="200">Returns all the existing Products</response>
        /// <response code="400">A bad request was sent</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var Products = await context.Products.ToListAsync();
            var ProductDtos = new List<ProductDto>();

            foreach (Product Product in Products)
            {
                ProductDtos.Add((ProductDto)Product);
            }

            return Ok(ProductDtos);
        }
        
        /// <summary>
        /// Get a specific Product by ID
        /// </summary>
        /// <param name="id">The id of the Product</param>
        /// <returns>a specific Product by ID</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET: api/Product/5
        ///
        /// </remarks>
        /// <response code="200">Returns the existing Product</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The Product was not found</response>
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ApiConventionMethod(typeof(CustomWebApiConventions), nameof(CustomWebApiConventions.Find))] // <- https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#apply-web-api-conventions
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> FindProduct(int id)
        {
            Product? Product = await context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }
            
            ProductDto ProductDto = (ProductDto)Product;
            return Ok(ProductDto);
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a specific Product by ID
        /// </summary>
        /// <param name="id">The id of the Product</param>
        /// <param name="ProductDto">the Product to update</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT: api/Product/5
        ///
        /// </remarks>
        /// <response code="204">The Product was updated successfully</response>
        /// <response code="400">The Product's Id doesn't match the rest of the Product's information entered</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The Product was not found</response>
        [Produces("application/json")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))] // <- https://learn.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-8.0#apply-web-api-conventions
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto ProductDto)
        {
            Product? ProductInDatabase = await context.Products.FirstOrDefaultAsync(
                Product =>
                Product.Name == ProductDto.Name && 
                Product.Description == ProductDto.Description && 
                Product.Category.ToString() == ProductDto.Category &&
                Product.Price == ProductDto.Price);

            if (ProductInDatabase == null)
                return NotFound();
            
            if (id != ProductInDatabase.Id)
                return BadRequest();

            context.SetModified(ProductInDatabase);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new Product
        /// </summary>
        /// <param name="ProductDto">The Product to create</param>
        /// <returns>The newly created Product</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST: api/Product
        ///
        /// </remarks>
        // /// <response code="201">The Product was created successfully</response>
        // /// <response code="400">A bad request was sent</response>
        [Produces("application/json")]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(201, "The Product was created", typeof(ProductDto))] // <-https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-response-metadata
        [SwaggerResponse(400, "The Product data is invalid")]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct([FromBody, SwaggerRequestBody("The Product payload", Required = true)]ProductDto ProductDto) // <- https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README.md#enrich-requestbody-metadata
        {
            Product databaseProduct = (Product)ProductDto;
            context.Products.Add(databaseProduct);
            await context.SaveChangesAsync();

            return CreatedAtAction("FindProduct", new { id = databaseProduct.Id }, databaseProduct);
        }
        
        /// <summary>
        /// Deletes a specific Product by ID
        /// </summary>
        /// <param name="id">The id of the Product to delete</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE: api/Product/5
        ///
        /// </remarks>
        /// <response code="204">The Product was deleted successfully</response>
        /// <response code="400">A bad request was sent</response>
        /// <response code="404">The Product was not found</response>
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Product? Product = await context.Products.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            context.Products.Remove(Product);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
