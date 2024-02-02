using System.ComponentModel.DataAnnotations;

namespace InMemoryCaching.API.EntityFramework.Entities;

public class Course
{
    [Key]
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public GradeLevel? GradeLevel { get; set; }

    [Range(1, 4, ErrorMessage = "A course must be between 1 and 4 credits")]
    public int Credits { get; set; }
    
    public ICollection<Enrollment> Enrollments { get; set; }
}