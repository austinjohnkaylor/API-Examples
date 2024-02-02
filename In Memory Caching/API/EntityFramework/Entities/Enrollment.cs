namespace InMemoryCaching.API.EntityFramework.Entities;

public class Enrollment
{
    public Guid EnrollmentId { get; set; }
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public GradeLetter? Grade { get; set; }
    public Course Course { get; set; }
    public Student Student { get; set; }
}