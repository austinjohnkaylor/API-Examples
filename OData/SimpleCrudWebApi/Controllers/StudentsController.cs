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
        public ActionResult<IQueryable<Student>> Get()
        {
            return Ok(context.Students);
        }

        /// <summary>
        /// Find a <see cref="Student"/> by it's identifier.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult<Student> Get([FromRoute] int key)
        {
            Student? student = context.Students.Find(key);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // // This attribute is responsible for applying the query options that are passed in the query string
        // [EnableQuery]
        // public ActionResult Post([FromBody] Student student)
        // {
        //     context.Students.Add(student);
        //     context.SaveChanges();
        //     return Created(student);
        // }
        //
        // // This attribute is responsible for applying the query options that are passed in the query string
        // [EnableQuery]
        // public ActionResult Put([FromODataUri] int key, [FromBody] Student student)
        // {
        //     student.StudentId = key;
        //     context.Students.Update(student);
        //     context.SaveChanges();
        //     return Updated(student);
        // }
        //
        // // This attribute is responsible for applying the query options that are passed in the query string
        // [EnableQuery]
        // public ActionResult Patch([FromODataUri] int key, [FromBody] Delta<Student> student)
        // {
        //     Student? entity = context.Students.Find(key);
        //     student.Patch(entity);
        //     context.SaveChanges();
        //     return Updated(entity);
        // }
        //
        // // This attribute is responsible for applying the query options that are passed in the query string
        // [EnableQuery]
        // public ActionResult Delete([FromODataUri] int key)
        // {
        //     Student? entity = context.Students.Find(key);
        //     context.Students.Remove(entity);
        //     context.SaveChanges();
        //     return NoContent();
        // }
    }
}
