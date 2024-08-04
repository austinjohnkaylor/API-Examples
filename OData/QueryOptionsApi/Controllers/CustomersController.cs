using API.Examples.OData.QueryOptionsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace API.Examples.OData.QueryOptionsApi.Controllers;

public class CustomersController : ODataController
{
    private static readonly List<Order> Orders =
    [
        new Order { Id = 1001, Price = 10, Quantity = 10 },
        new Order { Id = 1002, Price = 35, Quantity = 2 },
        new Order { Id = 1003, Price = 70, Quantity = 5 },
        new Order { Id = 1004, Price = 20, Quantity = 20 },
        new Order { Id = 1005, Price = 40, Quantity = 15 },
        new Order { Id = 1006, Price = 15, Quantity = 50 }
    ];

    private static readonly List<Customer> Customers =
    [
        new Customer { Id = 1, Name = "Customer 1", Age = 31, Orders = [Orders[0], Orders[1]] },
        new Customer { Id = 2, Name = "Customer 2", Age = 32, Orders = [Orders[2], Orders[3]] },
        new Customer { Id = 3, Name = "Customer 3", Age = 33, Orders = [Orders[4], Orders[5]] }
    ];
         
    /// <summary>
    ///  Get all <see cref="Customers"/>
    /// </summary>
    /// <returns></returns>
    /// <remarks>When the controller returns an IQueryable or IActionResult type, the LINQ provider converts the LINQ expression into a query, e.g., Entity Framework (EF) Core will convert the LINQ expression into an SQL statement</remarks>
    [EnableQuery] // an action filter that parses, validates, and applies the OData query parameters to the query
    public IActionResult Get()
    {
        return Ok(Customers);
    }
}