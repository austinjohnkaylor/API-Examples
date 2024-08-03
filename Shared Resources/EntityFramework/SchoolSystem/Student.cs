namespace API.Examples.SharedResources.EntityFramework.SchoolSystem;

public class Student
{
    public int StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public GradeLevel GradeLevel { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } // Navigation property
}