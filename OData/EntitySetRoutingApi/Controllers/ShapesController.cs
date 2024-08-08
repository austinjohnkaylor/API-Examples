using EntitySetRoutingApi.Models;
using Microsoft.AspNetCore.Mvc;
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
}