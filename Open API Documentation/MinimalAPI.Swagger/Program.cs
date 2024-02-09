using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configures the app to use the API Explorer to discover and describe endpoints with default annotations
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // adds the Swagger middleware
    app.UseSwaggerUI(); // enables an embedded version of the Swagger UI tool
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecasts", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecasts") // The IEndpointNameMetadata on the endpoint is used for link generation and is treated as the operation ID in the given endpoint's OpenAPI specification
    .WithOpenApi(openApiOperation =>
    {
        openApiOperation.Description = "Description";
        openApiOperation.Summary = "Summary";
        openApiOperation.Tags = new List<OpenApiTag> { new() { Name = "WeatherForecastGroup" } };
        return openApiOperation;
    })
    .Produces<WeatherForecast[]>();

app.MapGet("/weatherforecasts/{id:int}", (int id) =>
    {
        WeatherForecast forecast = new
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(id)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        );
        return TypedResults.Ok(forecast);
    })
    .WithOpenApi(operation =>
    {
        operation.Description = "This is the description";
        operation.Summary = "This is the summary";
        // description is added to the first parameter of the endpoint
        OpenApiParameter? parameter = operation.Parameters[0];
        parameter.Description = "The ID associated with the Weather Forecast";
        return operation;
    })
    .WithTags("WeatherForecasters"); // typically used to group operations in the Swagger UI

app.MapGet("/skipme", () => "Skipping Swagger.")
    .ExcludeFromDescription(); // excludes the endpoint from swagger

app.MapGet("/deprecated", () => "I am deprecated")
    .WithOpenApi(operation => new OpenApiOperation(operation)
    {
        // The endpoint shows up as deprecated in the API
        Deprecated = true
    });

app.MapGet("/multiple-response-types/{integer:int}", (int integer) =>
{
    return integer > 0 ? Results.Ok(true) : Results.NotFound(false);
}).Produces<bool>().Produces(StatusCodes.Status404NotFound);

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}