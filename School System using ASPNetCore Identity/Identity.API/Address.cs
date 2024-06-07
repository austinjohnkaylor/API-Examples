namespace Identity.API;

/// <summary>
/// Represents an address inside the United States
/// </summary>
/// <param name="Street"></param>
/// <param name="City"></param>
/// <param name="State"></param>
/// <param name="ZipCode"></param>
public record Address(string Street, string City, string State, string ZipCode);