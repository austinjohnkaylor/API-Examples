using System.ComponentModel.DataAnnotations;

namespace InMemoryCaching.API.EntityFramework.Entities;

/// <summary>
/// Represents a typical high-school student
/// </summary>
public class Student
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string? FirstName { get; set; }
    
    [Required]
    public string? MiddleName { get; set; }
    
    [Required]
    public string? LastName { get; set; }
    
    [Range(10, 20, ErrorMessage = "If you are less than 10 years old or older than 20 years old, you're either a genius or need to focus on getting your G.E.D")]
    public int Age { get; set; }

    public GradeLevel? Grade { get; set; }
    
    public Gender? Gender { get; set; }

    public string Email => $"{FirstName}.{MiddleName}.{LastName}@school.edu";
    
    public ICollection<Enrollment> Enrollments { get; set; }

}