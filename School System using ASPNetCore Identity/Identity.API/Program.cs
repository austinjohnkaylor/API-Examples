using Identity.API;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityApiEndpoints<SchoolSystemUser>()
    .AddEntityFrameworkStores<IdentityDatabaseContext>();

builder.Services.AddDbContext<IdentityDatabaseContext>(
    options => options.UseInMemoryDatabase("Identity"));

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Maps the following ASP.NET Core Identity endpoints to the API:
// - POST /register
// - POST /login
// - POST /refresh
// - GET /confirmEmail
// - POST /resendConfirmationEmail
// - POST /forgotPassword
// - POST /resetPassword
// - POST /manage/2fa
// - GET /manage/info
// - POST /manage/info
app.MapIdentityApi<SchoolSystemUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
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
    .WithName("GetWeatherForecast")
    .WithOpenApi()
    .RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}