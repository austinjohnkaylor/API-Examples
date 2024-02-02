using System.ComponentModel.DataAnnotations;

namespace InMemoryCaching.API.EntityFramework.Entities;

public class Teacher
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
    
    [Range(22, 99, ErrorMessage = "99 and still working? You deserve retirement friend!")]
    public int Age { get; set; }
    
    public Gender Gender { get; set; } = Gender.NonBinary;

    public string Email => $"Professor.{LastName}@school.edu";
}