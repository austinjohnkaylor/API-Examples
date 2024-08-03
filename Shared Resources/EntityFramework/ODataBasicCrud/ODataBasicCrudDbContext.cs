using Microsoft.EntityFrameworkCore;

namespace API.Examples.SharedResources.EntityFramework.ODataBasicCrud;

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
/// <remarks>https://learn.microsoft.com/en-us/odata/webapi-8/tutorials/basic-crud?tabs=net60%2Cvisual-studio-2022%2Cvisual-studio%2Cvs2022#add-a-database-context</remarks>
public class ODataBasicCrudDbContext(DbContextOptions<ODataBasicCrudDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}