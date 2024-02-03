using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InMemoryCaching.API.EntityFramework;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly PersonContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PersonController> _logger;
        public PersonController(PersonContext context, IMemoryCache cache, ILogger<PersonController> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
            InitializeInMemoryDatabase();
        }
        
        private const string PeopleCacheKey = "ListOfPeople";
        // Binary Semaphore. Only 1 thread can act on the cache at a given time
        private static readonly SemaphoreSlim SemaSlim = new(1, 1);
        
        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            IEnumerable<Person>? people;
            _logger.Log(LogLevel.Information, "Trying to fetch the list of people from cache.");

            if (_cache.TryGetValue(PeopleCacheKey, out people))
            {
                _logger.Log(LogLevel.Information, "List of people found in cache.");
                return Ok(people);
            }
            
            try
            {
                // Try to get people from cache
                await SemaSlim.WaitAsync();
            
                if (_cache.TryGetValue(PeopleCacheKey, out people))
                {
                    _logger.Log(LogLevel.Information, "List of people found in cache.");
                }
                else
                {
                    _logger.Log(LogLevel.Information, "List of people not found in cache. Fetching from database.");
            
                    people = await _context.People.ToListAsync();
            
                    MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                        // This sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
                        // In this case, the cache entry will expire if it is not accessed for 60 seconds
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        // This sets an absolute expiration time, relative to now.
                        // In this case, the cache entry will expire after 3600 seconds, regardless of whether it is accessed or not
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        // This sets the priority for keeping the cache entry in the cache during a memory pressure triggered cleanup.
                        // In this case, the cache entry has a normal priority, which means it has a medium chance of being evicted when the cache is full
                        .SetPriority(CacheItemPriority.Normal);
            
                    _cache.Set(PeopleCacheKey, people, cacheEntryOptions);
                }
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e.Message);
                throw;
            }
            finally
            {
                SemaSlim.Release();
            }

            return Ok(people);
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            Person? person = await _context.People.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            Person? person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }

        /// <summary>
        /// Populate the database with data
        /// </summary>
        /// <remarks>I resorted to this because .HasData() wasn't working in <see cref="PersonContext"/>'s OnModelCreating method</remarks>
        private void InitializeInMemoryDatabase()
        {
            _context.Database.EnsureCreated();
            
            if (_context.People.Any())
                return;
            
            PersonGenerator.Generate(1000);

            _context.People.AddRange(PersonGenerator.People);

            _context.SaveChanges();
        }
    }
}
