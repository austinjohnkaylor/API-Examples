using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace QueryOptionsApi.IntegrationTests;

/// <summary>
/// A collection of extension methods for the <see cref="string"/> class
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Pretty prints a Json string
    /// </summary>
    /// <param name="json">The Json string you want to prettify</param>
    /// <returns>A prettified Json string</returns>
    /// <remarks>https://stackoverflow.com/a/30329731</remarks>
    public static string JsonPrettify(this string json)
    {
        using StringReader stringReader = new(json);
        using StringWriter stringWriter = new();
        JsonTextReader jsonReader = new(stringReader);
        JsonTextWriter jsonWriter = new(stringWriter) { Formatting = Formatting.Indented };
        jsonWriter.WriteToken(jsonReader);
        return stringWriter.ToString();
    }
    
    public static string FormatJson(this string json)
    {
        dynamic parsedJson = JsonConvert.DeserializeObject(json) ?? string.Empty;
        return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
    }
    
    public static string ToFormattedJsonString(this string unformattedJson)
    {
        JsonNode formattedJson = JsonNode.Parse(unformattedJson) ?? new JsonObject();
        return formattedJson.ToJsonString();
    }
    
    public static string ReplaceCarriageReturns(this string input)
    {
        string cleanedInput = input.Replace("\r\n", "");
        return cleanedInput;
    }
}