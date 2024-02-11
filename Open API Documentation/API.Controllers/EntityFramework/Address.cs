namespace API.Controllers.EntityFramework;

public class Address
{
    /// <summary>
    /// Road number and name
    /// </summary>
    /// <example>123 Happy Valley Road</example>
    public string Street { get; set; }
    /// <summary>
    /// P.O. Box or Apartment number
    /// </summary>
    /// <example>P.O. Box 123</example>
    public string? StreetLineTwo { get; set; }
    /// <summary>
    /// The city the address is located in
    /// </summary>
    /// <example>Pittsburgh</example>
    public string City { get; set; }
    /// <summary>
    /// The state the address is located in
    /// </summary>
    /// <example>Pennsylvania</example>
    public string State { get; set; }
    /// <summary>
    /// The zip code for the city and state
    /// </summary>
    /// <example>15122</example>
    public string ZipCode { get; set; }
    
}