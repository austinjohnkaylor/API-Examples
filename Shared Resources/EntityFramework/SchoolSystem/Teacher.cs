namespace API.Examples.SharedResources.EntityFramework.SchoolSystem;

public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public ICollection<Course> Courses { get; set; } // Navigation property
}