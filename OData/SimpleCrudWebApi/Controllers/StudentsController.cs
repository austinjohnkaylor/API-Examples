using API.Examples.SharedResources.EntityFramework.SchoolSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.SimpleCrudWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class StudentsController(SchoolSystemDbContext context) : ODataController
    {
        // This attribute is responsible for applying the query options that are passed in the query string
        [EnableQuery]
        public ActionResult Get()
        {
            return Ok(context.Students);
        }

        // This attribute is responsible for applying the query options that are passed in the query string
        [EnableQuery]
        public ActionResult Get([FromODataUri] int key)
        {
            return Ok(context.Students.Find(key));
        }

        // This attribute is responsible for applying the query options that are passed in the query string
        [EnableQuery]
        public ActionResult Post([FromBody] Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return Created(student);
        }

        // This attribute is responsible for applying the query options that are passed in the query string
        [EnableQuery]
        public ActionResult Put([FromODataUri] int key, [FromBody] Student student)
        {
            student.StudentId = key;
            context.Students.Update(student);
            context.SaveChanges();
            return Updated(student);
        }

        // This attribute is responsible for applying the query options that are passed in the query string
        [EnableQuery]
        public ActionResult Patch([FromODataUri] int key, [FromBody] Delta<Student> student)
        {
            Student? entity = context.Students.Find(key);
            student.Patch(entity);
            context.SaveChanges();
            return Updated(entity);
        }

        // This attribute is responsible for applying the query options that are passed in the query string
        [EnableQuery]
        public ActionResult Delete([FromODataUri] int key)
        {
            Student? entity = context.Students.Find(key);
            context.Students.Remove(entity);
            context.SaveChanges();
            return NoContent();
        }
    }
}
