using Microsoft.EntityFrameworkCore;

namespace InMemoryCaching.API.EntityFramework;

public class PersonContext : DbContext
{
    public DbSet<Person> People { get; set; }

    public PersonContext(DbContextOptions options) : base(options)
    {
        
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