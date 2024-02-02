using API.ActionFilters;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController(ILogger<PeopleController> logger) : ControllerBase
{
    private readonly ILogger<PeopleController> _logger = logger;

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        _logger.LogInformation($"Getting person with Id {id}");
        var person = new Person
        {
            Id = id,
            FirstName = "Jane",
            LastName = "Doe",
            Age = 30,
            Gender = Gender.Female
        };
        return Ok(person);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public IActionResult Post([FromBody] Person person)
    {
        _logger.LogInformation($"Creating a new person");
        return CreatedAtAction(nameof(Post), new { id = person.Id }, person);
    }
}