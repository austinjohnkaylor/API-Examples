using Microsoft.AspNetCore.Mvc;
using ModelStateValidation.API.ActionFilters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

// Added for integration tests
namespace ModelStateValidation.API
{
    public partial class Program;
}