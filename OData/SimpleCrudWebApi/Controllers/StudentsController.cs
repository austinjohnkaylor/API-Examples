using API.Examples.SharedResources.EntityFramework.SchoolSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.SimpleCrudWebApi.Controllers;

//[Route("api/[controller]")]
//[ApiController]
public class StudentsController(SchoolSystemDbContext context) : ODataController
{
    /// <summary>
    /// Get all <see cref="Student"/> records.
    /// </summary>
    /// <returns>All <see cref="Student"/> records</returns>
    // This attribute is responsible for applying the query options that are passed in the query string
    [EnableQuery]
    public ActionResult<IQueryable<Student>> Get()
    {
        return Ok(context.Students);
    }

    /// <summary>
    /// Find a <see cref="Student"/> by it's identifier.
    /// </summary>
    /// <param name="key">The Id of the Student being searched for</param>
    /// <returns>A <see cref="Student"/> with an Id of <see cref="key"/></returns>
    public ActionResult<Student> Get([FromRoute] int key)
    {
        Student? student = context.Students.Find(key);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }
        
    public ActionResult Post([FromBody] Student student)
    {
        context.Students.Add(student);
        context.SaveChanges();
        return Created(student);
    }
        
    /// <summary>
    /// Updates a <see cref="Student"/> in it's entirety by it's identifier.
    /// </summary>
    /// <param name="key">The Id of the <see cref="Student"/> being updated</param>
    /// <param name="student">The completely updated <see cref="Student"/> record</param>
    /// <returns></returns>
    public ActionResult Put([FromODataUri] int key, [FromBody] Student student)
    {
        student.Id = key;
        context.Students.Update(student);
        context.SaveChanges();
        return Updated(student);
    }
        
    /// <summary>
    /// Applies Delta changes to a <see cref="Student"/> by it's identifier.
    /// </summary>
    /// <param name="key">The Id of the <see cref="Student"/> being updated</param>
    /// <param name="student">A <see cref="Delta"/> of <see cref="Student"/> that tracks specific changes to the record and applies them</param>
    /// <returns></returns>
    public ActionResult Patch([FromODataUri] int key, [FromBody] Delta<Student> student)
    {
        Student? entity = context.Students.Find(key);
        student.Patch(entity);
        context.SaveChanges();
        return Updated(entity);
    }
        
    /// <summary>
    /// Deletes a <see cref="Student"/> by it's identifier.
    /// </summary>
    /// <param name="key">The Id of the <see cref="Student"/> being deleted</param>
    /// <returns></returns>
    public ActionResult Delete([FromODataUri] int key)
    {
        Student? entity = context.Students.Find(key);
        context.Students.Remove(entity);
        context.SaveChanges();
        return NoContent();
    }
}