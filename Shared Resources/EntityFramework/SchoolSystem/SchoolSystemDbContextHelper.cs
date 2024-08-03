using Bogus;

namespace API.Examples.SharedResources.EntityFramework.SchoolSystem;

public static class SchoolSystemDbContextHelper
{
    public static void SeedData(SchoolSystemDbContext context)
    {
        var studentFaker = new Faker<Student>()
            .RuleFor(s => s.Id, f => f.IndexFaker + 1)
            .RuleFor(s => s.FirstName, f => f.Name.FirstName())
            .RuleFor(s => s.LastName, f => f.Name.LastName())
            .RuleFor(s => s.Email, f => f.Internet.Email())
            .RuleFor(s => s.GradeLevel, f => f.PickRandom<GradeLevel>());

        var teacherFaker = new Faker<Teacher>()
            .RuleFor(t => t.Id, f => f.IndexFaker + 1)
            .RuleFor(t => t.FirstName, f => f.Name.FirstName())
            .RuleFor(t => t.LastName, f => f.Name.LastName())
            .RuleFor(t => t.Email, f => f.Internet.Email());

        var courseFaker = new Faker<Course>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Title, f => f.Lorem.Word())
            .RuleFor(c => c.Credits, f => f.Random.Int(1, 5))
            .RuleFor(c => c.TeacherId, f => f.PickRandom(context.Teachers.Select(t => t.Id).ToList()));

        var enrollmentFaker = new Faker<Enrollment>()
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.CourseId, f => f.PickRandom(context.Courses.Select(c => c.Id).ToList()))
            .RuleFor(e => e.StudentId, f => f.PickRandom(context.Students.Select(s => s.Id).ToList()));

        // Generate and add data to the context
        var students = studentFaker.Generate(50);
        var teachers = teacherFaker.Generate(10);
        context.Students.AddRange(students);
        context.Teachers.AddRange(teachers);
        context.SaveChanges();

        var courses = courseFaker.Generate(20);
        context.Courses.AddRange(courses);
        context.SaveChanges();

        var enrollments = enrollmentFaker.Generate(100);
        context.Enrollments.AddRange(enrollments);
        context.SaveChanges();
    }

}