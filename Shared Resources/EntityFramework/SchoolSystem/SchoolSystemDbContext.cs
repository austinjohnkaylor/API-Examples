using Microsoft.EntityFrameworkCore;

namespace API.Examples.SharedResources.EntityFramework.SchoolSystem;

public class SchoolSystemDbContext(DbContextOptions<SchoolSystemDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
}