using API.Examples.SharedResources.EntityFramework.SchoolSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.SimpleCrudWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(SchoolSystemDbContext context) : ODataController
    {
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(context.Students);
        }

        [EnableQuery]
        public IActionResult Get([FromODataUri] int key)
        {
            return Ok(context.Students.Find(key));
        }

        [EnableQuery]
        public IActionResult Post([FromBody] Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return Created(student);
        }

        [EnableQuery]
        public IActionResult Put([FromODataUri] int key, [FromBody] Student student)
        {
            student.StudentId = key;
            context.Students.Update(student);
            context.SaveChanges();
            return Updated(student);
        }

        [EnableQuery]
        public IActionResult Patch([FromODataUri] int key, [FromBody] Delta<Student> student)
        {
            var entity = context.Students.Find(key);
            student.Patch(entity);
            context.SaveChanges();
            return Updated(entity);
        }

        [EnableQuery]
        public IActionResult Delete([FromODataUri] int key)
        {
            var entity = context.Students.Find(key);
            context.Students.Remove(entity);
            context.SaveChanges();
            return NoContent();
        }
    }
}
