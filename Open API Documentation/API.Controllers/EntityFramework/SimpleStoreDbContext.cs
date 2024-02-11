namespace API.Controllers.EntityFramework;

public class SimpleStoreDbContext : DbContext
{
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    
    public SimpleStoreDbContext() { }

    public SimpleStoreDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities?source=recommendations#mapping-owned-types-with-table-splitting
        modelBuilder.Entity<Customer>()
            .HasBaseType<Audit>() // https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#entity-type-hierarchy-mapping
            .OwnsOne(
                o => o.Address,
                sa =>
                {
                    sa.Property(p => p.Street).HasColumnName("Street");
                    sa.Property(p => p.StreetLineTwo).HasColumnName("StreetLineTwo");
                    sa.Property(p => p.City).HasColumnName("City");
                    sa.Property(p => p.State).HasColumnName("State");
                    sa.Property(p => p.ZipCode).HasColumnName("ZipCode");
                });
        
        // https://learn.microsoft.com/en-us/ef/core/modeling/owned-entities?source=recommendations#mapping-owned-types-with-table-splitting
        modelBuilder.Entity<Order>()
            .HasBaseType<Audit>() // https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#entity-type-hierarchy-mapping
            .OwnsOne(
            o => o.ShippingAddress,
            sa =>
            {
                sa.Property(p => p.Street).HasColumnName("ShipsToStreet");
                sa.Property(p => p.StreetLineTwo).HasColumnName("ShipsToStreetLineTwo");
                sa.Property(p => p.City).HasColumnName("ShipsToCity");
                sa.Property(p => p.State).HasColumnName("ShipsToState");
                sa.Property(p => p.ZipCode).HasColumnName("ShipsToZipCode");
            });
        
        modelBuilder.Entity<Product>()
            .HasBaseType<Audit>(); // https://learn.microsoft.com/en-us/ef/core/modeling/inheritance#entity-type-hierarchy-mapping
        
        base.OnModelCreating(modelBuilder);
    }

    public virtual void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }
}