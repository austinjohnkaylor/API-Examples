using API.Examples.SharedResources.EntityFramework.SchoolSystem;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace API.Examples.OData.SimpleCrudWebApi;

public static class SchoolSystemODataEdmModelBuilder
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnumType<GradeLevel>();
        var students = builder.EntitySet<Student>("Students");
        students.EntityType.Ignore(student => student.Email); // Ignore the Email property --> https://learn.microsoft.com/en-us/odata/webapi/odata-security#edm-security
        builder.EntitySet<Teacher>("Teachers");
        builder.EntitySet<Course>("Courses");
        builder.EntitySet<Enrollment>("Enrollments");
        return builder.GetEdmModel();
    }
}