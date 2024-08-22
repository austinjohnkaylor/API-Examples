using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;

namespace API.Examples.OData.IntegrationTests.Extensions;

[ExcludeFromCodeCoverage]
public static class StringExtensions
{
    public static string ToFormattedJsonString(this string unformattedJson)
    {
        JsonNode formattedJson = JsonNode.Parse(unformattedJson) ?? new JsonObject();
        return formattedJson.ToJsonString();
    }
}