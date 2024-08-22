namespace API.Examples.SharedResources.EntityFramework.SchoolSystem;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } // Navigation property
    public ICollection<Enrollment> Enrollments { get; set; } // Navigation property
}