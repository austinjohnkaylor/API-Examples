namespace API.Examples.SharedResources.EntityFramework.SchoolSystem;

public class Enrollment
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public Course Course { get; set; } // Navigation property
    public Student Student { get; set; } // Navigation property
}