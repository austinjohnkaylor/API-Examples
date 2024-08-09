using System.Reflection;
using EntitySetRoutingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace EntitySetRoutingApi.Controllers;

public class ShapesController : ODataController
{
    public static readonly List<Shape> Shapes =
    [
        new Rectangle { Id = 1, Length = 7, Width = 4, Area = 28 },
        new Circle { Id = 2, Radius = 3.5, Area = 38.5 },
        new Rectangle { Id = 3, Length = 8, Width = 5, Area = 40 }
    ];
    
    [EnableQuery]
    public ActionResult<IEnumerable<Shape>> Get()
    {
        return Shapes;
    }
    
    [EnableQuery]
    public ActionResult<IEnumerable<Rectangle>> GetFromRectangle()
    {
        return Shapes.OfType<Rectangle>().ToList();
    }
    
    public ActionResult Post([FromBody] Shape shape)
    {
        Shapes.Add(shape);

        // generates the location at which the resource has been created and returns it as a response header
        return Created(shape);
    }
    
    public ActionResult PostFromCircle([FromBody] Circle circle)
    {
        Shapes.Add(circle);

        return Created(circle);
    }
    
    /// <summary>
    /// The semantics of PATCH are to merge the content in the request payload with the entity's or entities' current state, applying the update only to those components specified in the request body. 
    /// </summary>
    /// <param name="deltaSet"></param>
    /// <remarks>To patch a collection of entities in an entity set, the client sends a PATCH request to that entity set's URL</remarks>
    /// <returns></returns>
    public ActionResult Patch([FromBody] DeltaSet<Shape> deltaSet)
    {
        foreach (IDeltaSetItem? deltaSetItem in deltaSet)
        {
            var delta = (Delta<Shape>)deltaSetItem;
            if (!delta.TryGetPropertyValue("Id", out object idAsObject)) continue;
            Shape? shape = Shapes.SingleOrDefault(d => d.Id.Equals(idAsObject));
            if (shape != null) delta.Patch(shape);
        }

        return NoContent();
    }
    
    /// <summary>
    /// To patch a collection of derived entities in an entity set, the client sends a PATCH request to that entity set's URL with the fully-qualified name of the derived type appended at the end.
    /// </summary>
    /// <param name="deltaSet"></param>
    /// <returns></returns>
    public ActionResult PatchFromRectangle([FromBody] DeltaSet<Rectangle> deltaSet)
    {
        foreach (IDeltaSetItem? deltaSetItem in deltaSet)
        {
            var delta = (Delta<Rectangle>)deltaSetItem;
            if (!delta.TryGetPropertyValue("Id", out object idAsObject)) continue;
            if (Shapes.SingleOrDefault(d => d.Id.Equals(idAsObject)) is Rectangle rectangle) delta.Patch(rectangle);
        }

        return NoContent();
    }
    
    public ActionResult<Shape> Get([FromRoute] int key)
    {
        Shape? shape = Shapes.SingleOrDefault(d => d.Id.Equals(key));

        if (shape == null)
            return NotFound();
        

        return shape;
    }
    
    /// <summary>
    /// Gets a circle by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public ActionResult<Circle> GetCircle([FromRoute] int key)
    {
        Circle? circle = Shapes.OfType<Circle>().SingleOrDefault(d => d.Id.Equals(key));

        if (circle == null)
        {
            return NotFound();
        }

        return circle;
    }
    
    /// <summary>
    /// Can be Put or PutShape
    /// </summary>
    /// <param name="key">Key of the target entity</param>
    /// <param name="shape">The shape from the request body being updated</param>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#updating-a-single-derived-entity</remarks>
    /// <returns></returns>
    public ActionResult Put([FromRoute] int key, [FromBody] Shape shape)
    {
        Shape? item = Shapes.SingleOrDefault(d => d.Id.Equals(key));

        if (item == null)
        {
            return NotFound();
        }

        if (item.GetType() != shape.GetType())
        {
            return BadRequest();
        }

        // Update properties using reflection
        foreach (PropertyInfo propertyInfo in shape.GetType().GetProperties(
                     BindingFlags.Public | BindingFlags.Instance))
        {
            PropertyInfo? itemPropertyInfo = item.GetType().GetProperty(
                propertyInfo.Name,
                BindingFlags.Public | BindingFlags.Instance);

            if (itemPropertyInfo != null && itemPropertyInfo.CanWrite)
            {
                itemPropertyInfo.SetValue(item, propertyInfo.GetValue(shape));
            }
        }

        return NoContent();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="circle"></param>
    /// <returns></returns>
    public ActionResult PutCircle([FromRoute] int key, [FromBody] Circle circle)
    {
        Circle? item = Shapes.OfType<Circle>().SingleOrDefault(d => d.Id.Equals(key));

        if (item == null)
        {
            return NotFound();
        }

        item.Id = circle.Id;
        item.Radius = circle.Radius;
        item.Area = circle.Area;

        return NoContent();
    }
    
    /// <summary>
    /// Patches a shape by key along with the delta of the shape passed in the request body
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#patching-a-single-entity</remarks>
    public ActionResult Patch([FromRoute] int key, [FromBody] Delta<Shape> delta)
    {
        Shape? shape = Shapes.SingleOrDefault(d => d.Id.Equals(key));

        if (shape == null)
        {
            return NotFound();
        }

        if (shape.GetType() != delta.StructuredType)
        {
            return BadRequest();
        }

        delta.Patch(shape);

        return NoContent();
    }
    
    /// <summary>
    /// Patch a circle by key along with the delta of the circle passed in the request body
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#patching-a-single-derived-entity</remarks>
    /// <param name="key"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    public ActionResult PatchCircle([FromRoute] int key, [FromBody] Delta<Circle> delta)
    {
        Circle? shape = Shapes.OfType<Circle>().SingleOrDefault(d => d.Id.Equals(key));

        if (shape == null)
        {
            return NotFound();
        }

        delta.Patch(shape);

        return NoContent();
    }
    
    /// <summary>
    /// Deletes a shape by key
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#deleting-a-single-entity</remarks>
    /// <param name="key"></param>
    /// <returns></returns>
    public ActionResult DeleteShape([FromRoute] int key)
    {
        Shape? shape = Shapes.SingleOrDefault(d => d.Id.Equals(key));

        if (shape == null)
        {
            return NotFound();
        }

        Shapes.Remove(shape);

        return NoContent();
    }
    
    /// <summary>
    /// Deletes a circle by key
    /// </summary>
    /// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/fundamentals/entity-routing?tabs=net60%2Cvisual-studio#deleting-a-single-derived-entity</remarks>
    /// <param name="key"></param>
    /// <returns></returns>
    public ActionResult DeleteCircle([FromRoute] int key)
    {
        Circle? shape = Shapes.OfType<Circle>().SingleOrDefault(d => d.Id.Equals(key));

        if (shape == null)
        {
            return NotFound();
        }

        Shapes.Remove(shape);

        return NoContent();
    }
}