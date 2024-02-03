using Microsoft.EntityFrameworkCore;

namespace InMemoryCaching.API.EntityFramework;

public class PersonContext : DbContext
{
    public DbSet<Person> People { get; set; }

    public PersonContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().HasData(PersonGenerator.Generate(1000));
    }
}