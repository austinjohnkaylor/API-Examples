using EntitySetRoutingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace EntitySetRoutingApi.Controllers;

public class ShapesController : ODataController
{
    private static readonly List<Shape> Shapes =
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
}