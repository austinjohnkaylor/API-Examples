using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;

namespace API.Examples.SharedResources.EntityFramework;

public class EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
}