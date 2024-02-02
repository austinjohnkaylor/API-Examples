namespace InMemoryCaching.API.EntityFramework.Entities;

public class Course
{
    public string Title { get; set; }

    public string Description { get; set; }

    public GradeLevel GradeLevel { get; set; } = GradeLevel.Freshman;
}