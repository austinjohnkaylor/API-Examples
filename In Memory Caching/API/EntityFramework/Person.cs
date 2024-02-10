using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InMemoryCaching.API.EntityFramework;

/// <summary>
/// Represents a person
/// </summary>
public class Person
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required] 
    public int Id { get; set; }
    [Required]
    [MaxLength(50, ErrorMessage = "If your name is longer than 50 characters, you may want to consider a nickname")]
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Range(1,130, ErrorMessage = "No human has ever lived longer than 130 years!")]
    public int Age { get; set; }
    public Gender Gender { get; set; }
}

public enum Gender
{
    Male,
    Female
}