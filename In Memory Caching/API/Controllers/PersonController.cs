using System.Net;
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
        private const string PeopleCacheKey = "ListOfPeople";
        private const string PersonCacheKey = "Person_{0}"; // {0} is the employee id
        // Binary Semaphore. Only 1 thread can act on the cache at a given time
        private static readonly SemaphoreSlim SemaSlim = new(1, 1);
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
        
        public PersonController(PersonContext context, IMemoryCache cache, ILogger<PersonController> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
            _memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                // This sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
                // In this case, the cache entry will expire if it is not accessed for 60 seconds
                SlidingExpiration = TimeSpan.FromSeconds(60),
                // This sets an absolute expiration time, relative to now.
                // In this case, the cache entry will expire after 3600 seconds, regardless of whether it is accessed or not
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                // This sets the priority for keeping the cache entry in the cache during a memory pressure triggered cleanup.
                // In this case, the cache entry has a normal priority, which means it has a medium chance of being evicted when the cache is full
                Priority = CacheItemPriority.Normal
            };
        }
        
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
                    
                    _cache.Set(PeopleCacheKey, people, _memoryCacheEntryOptions);
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
            Person? person;
            string cacheKey = string.Format(PersonCacheKey, id);
            
            _logger.LogInformation("Trying to get person {Id} from the cache", id);
            if (_cache.TryGetValue(cacheKey, out person))
            {
                // Cache Hit
                _logger.LogInformation("Person {id} retrieved from the cache", id);
                return Ok(person);
            }
            
            _logger.LogInformation("Person {id} was not found in the cache. Fetching from the database instead", id);
            person = await _context.People.FindAsync(id);
            
            // Check if the employee exists
            if (person == null)
            {
                _logger.LogInformation("Person {id} not found in the cache or the database", id);
                return NotFound();
            }
            
            // Save the employee in the cache
            _logger.LogInformation("Saving Person {id} to the cache", id);
            _cache.Set(cacheKey, person, _memoryCacheEntryOptions);
            
            return Ok(person);
        }

        // PUT: api/Person/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            string cacheKey = string.Format(PersonCacheKey, id);

            if (id != person.Id)
            {
                _logger.LogInformation("Id {id} does not match Person's {personId}",id, person.Id);
                return BadRequest();
            }

            if (!PersonExists(id))
            {
                _logger.LogInformation("Person {id} not found in the cache or the database", id);
                return NotFound();
            }
            
            // Update the person in the database
            _logger.LogInformation("Updating Person {id} in the database", id);
            _context.Entry(person).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            // Remove the person from the in-memory cache
            _logger.LogInformation("Removing Person {id} with cache key {key} from the cache", id, cacheKey);
            _cache.Remove(cacheKey);

            return NoContent();
        }

        // POST: api/Person
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            string cacheKey = string.Format(PersonCacheKey, person.Id);

            if (PersonExists(person.Id))
            {
                _logger.LogInformation("Person {id} may already exist in the database", person.Id);
                return StatusCode(409, person);
            }
            
            _logger.LogInformation("Invalidating cache with key {key} because a new person was added", PeopleCacheKey);
            _cache.Remove(PeopleCacheKey);
            
            _logger.LogInformation("Adding Person {id} to database", person.Id);
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Adding Person {id} to cache with key {key}", person.Id, cacheKey);
            _cache.Set(cacheKey, person, _memoryCacheEntryOptions);

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            string cacheKey = string.Format(PersonCacheKey, id);

            Person? person = await _context.People.FindAsync(id);
            if (person == null)
            {
                _logger.LogInformation("Couldn't find Person {id} in the database", id);
                return NotFound();
            }
            
            _logger.LogInformation("Removing Person {id} and key {key} from cache", id, cacheKey);
            _cache.Remove(cacheKey);
            
            _logger.LogInformation("Invalidating cache with key {key} because a person was deleted", PeopleCacheKey);
            _cache.Remove(PeopleCacheKey);
            
            _logger.LogInformation("Removing Person {id} from the database", id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
