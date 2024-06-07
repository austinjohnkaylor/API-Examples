using Microsoft.AspNetCore.Identity;

namespace Identity.API;

/// <summary>
/// A custom <see cref="IdentityUser"/> for the school system
/// </summary>
public class SchoolSystemUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string MiddleName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
    [PersonalData]
    public int Age { get; set; }
    [PersonalData]
    public Address CurrentAddress { get; set; }
}