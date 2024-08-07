using System.Text.Json.Nodes;

namespace EntitySetRoutingApi.IntegrationTests;

/// <summary>
/// A collection of extension methods for the <see cref="string"/> class
/// </summary>
public static class StringExtensions
{
    public static string ToFormattedJsonString(this string unformattedJson)
    {
        JsonNode formattedJson = JsonNode.Parse(unformattedJson) ?? new JsonObject();
        return formattedJson.ToJsonString();
    }
}