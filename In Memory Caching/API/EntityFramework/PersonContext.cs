using Microsoft.EntityFrameworkCore;

namespace InMemoryCaching.API.EntityFramework;

public class PersonContext : DbContext
{
    public virtual DbSet<Person> People { get; set; }

    public PersonContext()
    {
        
    }

    public PersonContext(DbContextOptions options) : base(options)
    {
        
    }
    public virtual void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }
    

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //
    //     PersonGenerator.Generate(1000);
    //     
    //     modelBuilder.Entity<Person>().HasData(PersonGenerator.People);
    // }
}